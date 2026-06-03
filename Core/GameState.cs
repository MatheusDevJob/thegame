using System.Linq;
using Microsoft.Xna.Framework;
using thegame.Entities;
using thegame.Items;
using thegame.Maps;

namespace thegame.Core;

public class GameState
{
    private readonly GameContext _context;

    public GameSave PlayerSave { get; private set; }
    public Player Player { get; private set; }
    public Inventory Inventory { get; private set; } = new();
    public Entity ActiveEquipe { get; set; }
    public bool LayoutMenu = false;
    public bool LayoutBag = false;

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

    private void SyncSaveFromRuntime()
    {
        PlayerSave.PlayerPosition = Player.Posicao;
        PlayerSave.PlayerLife = Player.Life;
        PlayerSave.BagLevel = Inventory.BagLevel;
        PlayerSave.BagItems = Inventory.ToSaveItems();

        if (ActiveEquipe != null)
            PlayerSave.ActiveEquipe = ActiveEquipe.Id;
    }

    public void SaveGame()
    {
        SyncSaveFromRuntime();
        SaveManager.Save(PlayerSave);
    }

    public bool PlayerHasTool(string toolId)
    {
        return PlayerSave.EquipableIds != null && PlayerSave.EquipableIds.Contains(toolId);
    }

    public void AddTool(string toolId)
    {
        if (string.IsNullOrWhiteSpace(toolId))
            return;

        PlayerSave.EquipableIds ??= [];

        if (!PlayerSave.EquipableIds.Contains(toolId))
            PlayerSave.EquipableIds.Add(toolId);

        if (string.IsNullOrWhiteSpace(PlayerSave.ActiveEquipe))
            SetActiveEquipe(toolId);
    }

    public void SetActiveEquipe(string toolId)
    {
        if (string.IsNullOrWhiteSpace(toolId))
            return;

        if (!PlayerHasTool(toolId))
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

    public void AddItemToBag(string itemId, int amount = 1)
    {
        Inventory.AddItem(itemId, itemId, amount);
    }

    public bool RemoveItemFromBag(string itemId, int amount = 1)
    {
        return Inventory.RemoveItem(itemId, amount);
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

    public void AddDroppedItem(string mapId, string saveId, string itemId, int amount, Vector2 position)
    {
        if (string.IsNullOrWhiteSpace(mapId) || string.IsNullOrWhiteSpace(saveId) || string.IsNullOrWhiteSpace(itemId) || amount <= 0)
            return;

        MapSave mapSave = PlayerSave.GetMapSave(mapId);

        bool alreadyExists = mapSave.DroppedItems.Any(i => i.SaveId == saveId);

        if (alreadyExists)
            return;

        mapSave.DroppedItems.Add(new WorldItemSave
        {
            SaveId = saveId,
            ItemId = itemId,
            Amount = amount,
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
}