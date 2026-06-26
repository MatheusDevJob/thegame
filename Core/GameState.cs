using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using thegame.Entities;
using thegame.Entities.WorldObjects.Interactables;
using thegame.Items;
using thegame.Maps;
using thegame.Scenes;

namespace thegame.Core;

public class GameState
{
    private readonly GameContext _context;
    public double TempoJogoSegundos { get; set; }

    public GameSave PlayerSave { get; private set; }
    public Player Player { get; private set; }
    public Inventory Inventory { get; private set; } = new();
    public Entity ActiveEquipe { get; set; }
    public Entity EntidadeEmFoco { get; set; }
    public bool LayoutMenu = false;
    public bool LayoutBag = false;
    public readonly EntityWorld EntityWorld = new();
    public GameScene GameScene { get; set; }
    public TiledMap TiledMap { get; set; }
    public string MapaAtualId { get; set; }

    public GameState(GameContext context, GameSave gameSave)
    {
        _context = context;
        PlayerSave = gameSave;
        Player = new Player(context, gameSave);

        LoadRuntimeFromSave();
    }

    private void LoadRuntimeFromSave()
    {
        Inventory.LoadFromSave(PlayerSave.BagItems, PlayerSave.BagLevel);
        SetActiveEquipe(PlayerSave.ActiveEquipe);
    }

    public void SyncSaveFromRuntime()
    {
        PlayerSave.PlayerPosition = Player.Posicao;
        PlayerSave.PlayerLife = Player.Life;
        PlayerSave.PlayerCarteira = Player.Carteira;
        PlayerSave.BagLevel = Inventory.BagLevel;
        PlayerSave.BagItems = Inventory.ToSaveItems();

        if (ActiveEquipe != null)
            PlayerSave.ActiveEquipe = ActiveEquipe.Id;

        SyncMapEntity();
    }

    public void SyncMapEntity()
    {
        MapSave mapSave = PlayerSave.GetMapSave(PlayerSave.CurrentMap);

        mapSave.SpawnedEntities.Clear();

        foreach (Entity item in EntityWorld._entities)
        {
            if (!item.Persistente)
                continue;

            mapSave.SpawnedEntities.Add(new WorldEntitySave
            {
                SaveId = item.SaveId,
                EntityId = item.Id,
                X = item.Posicao.X,
                Y = item.Posicao.Y,
                Data = AtualizarDataBau(item)
            });
        }
    }
    public static Dictionary<string, string> AtualizarDataBau(Entity item)
    {
        if (item is Bau bau)
            item.Data = bau.Items;
        return new Dictionary<string, string>
        {
            ["items"] = JsonSerializer.Serialize(item.Data)
        };
    }

    public void SaveGame()
    {
        SyncSaveFromRuntime();
        SaveManager.Save(PlayerSave);
    }

    public void AddTool(string toolId)
    {
        if (string.IsNullOrWhiteSpace(toolId))
            return;

        // PlayerSave.EquipableIds ??= [];

        // if (!PlayerSave.EquipableIds.Contains(toolId))
        //     PlayerSave.EquipableIds.Add(toolId);

        if (string.IsNullOrWhiteSpace(PlayerSave.ActiveEquipe))
            SetActiveEquipe(toolId);
    }

    public void SetActiveEquipe(string toolId)
    {
        if (string.IsNullOrWhiteSpace(toolId))
            return;

        Entity tool = EntityFactory.Create(_context, new TiledObjectData
        {
            Type = toolId,
            X = 0,
            Y = 0
        });

        if (tool == null)
            return;

        ActiveEquipe = tool;
        PlayerSave.ActiveEquipe = toolId;
    }

    public void AddItemToBag(string itemId, int quantidade = 1)
    {
        Inventory.AddItem(itemId, quantidade);
    }

    public bool RemoveItemFromBag(string itemId, int quantidade = 1)
    {
        return Inventory.RemoveItem(itemId, quantidade);
    }

    public void MarkEntityRemoved(string mapId, string saveId)
    {
        if (string.IsNullOrWhiteSpace(mapId) || string.IsNullOrWhiteSpace(saveId))
            return;

        MapSave mapSave = PlayerSave.GetMapSave(mapId);

        if (!mapSave.RemovedEntities.Contains(saveId))
            mapSave.RemovedEntities.Add(saveId);

        mapSave.SpawnedEntities.RemoveAll(e => e.SaveId == saveId);
    }

    public bool IsEntityRemoved(string mapId, string saveId)
    {
        if (string.IsNullOrWhiteSpace(mapId) || string.IsNullOrWhiteSpace(saveId))
            return false;

        MapSave mapSave = PlayerSave.GetMapSave(mapId);
        return mapSave.RemovedEntities.Contains(saveId);
    }

    public void AddDroppedItem(string mapId, string saveId, string itemId, int quantidade, Vector2 position)
    {
        if (string.IsNullOrWhiteSpace(mapId) || string.IsNullOrWhiteSpace(saveId) || string.IsNullOrWhiteSpace(itemId) || quantidade <= 0)
            return;

        MapSave mapSave = PlayerSave.GetMapSave(mapId);

        bool alreadyExists = mapSave.DroppedItems.Any(i => i.SaveId == saveId);

        if (alreadyExists)
            return;

        mapSave.DroppedItems.Add(new WorldItemSave
        {
            SaveId = saveId,
            ItemId = itemId,
            Quantidade = quantidade,
            X = position.X,
            Y = position.Y
        });
    }

    public void RemoveDroppedItem(string mapId, string saveId)
    {
        if (string.IsNullOrWhiteSpace(mapId) || string.IsNullOrWhiteSpace(saveId))
            return;

        MapSave mapSave = PlayerSave.GetMapSave(mapId);
        mapSave.DroppedItems.RemoveAll(i => i.SaveId == saveId);
    }

    // controle dos eventos do estado do jogo
    public bool StartEvent(string Evento)
    {
        switch (Evento)
        {
            case "Caverna":
                GameScene.ChangeMap(new Caverna(_context));
                return true;

            case "City":
                GameScene.ChangeMap(new CityMap(_context));
                return true;

            default:
                return false;
        }
    }

    public void SetPosicaoPlayerMapa(string Evento)
    {
        TiledLayerData Layers = TiledMap.GetObjects("Portal");
        TiledObjectData tiledObjectDatas = Layers.Objects.FirstOrDefault((e) => e.Type == "Portal" && e.Name == Evento);
        if (tiledObjectDatas != null)
        {
            Player.DefinirPosicao(new Vector2(tiledObjectDatas.X, tiledObjectDatas.Y));
        }
    }

    // Estados do caminhão de venda
    public CaminhaoVendaState Venda { get; set; } = new();
}

public enum CaminhaoVendaStatus
{
    Disponivel,
    EmRota,
    AguardandoColeta
}

public class CaminhaoVendaState
{
    public Guid LojaId { get; set; }
    public CaminhaoVendaStatus Status { get; set; } = CaminhaoVendaStatus.Disponivel;
    public double RetornaEmTempoJogo { get; set; }
    public int DinheiroPendente { get; set; }
    public List<ItemStackSave> ItensEnviados { get; set; } = [];
}