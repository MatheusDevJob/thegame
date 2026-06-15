using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.WorldObjects.Interactables;
using thegame.Entities.WorldObjects.Solo;

namespace thegame.Maps;

public class WorldActionService(GameContext context, EntityWorld entityWorld, string mapId)
{
    private static readonly HashSet<string> DiggableTerrains = new(StringComparer.OrdinalIgnoreCase)
        {
            "Grass",
            "Sand",
            "Dirt"
        };
    private string TerrainType;
    public bool IsDiggable(string terrainType)
    {
        return !string.IsNullOrEmpty(terrainType) && DiggableTerrains.Contains(terrainType) && context.State.ActiveEquipe.Id == "ShovelTool";
    }

    public void DestroyEntity(Entity entity)
    {
        if (entity == null)
            return;

        if (entity.Persistente)
            context.State.MarkEntityRemoved(mapId, entity.SaveId);

        entityWorld.Remove(entity);
    }

    public void OnTileClicked(Point tile, TiledMap Map)
    {
        string layer = "Ground";
        TerrainType = Map.GetTilePropertyString(layer, tile, "terrainType");

        if (string.IsNullOrEmpty(TerrainType)) return;

        switch (context.State.ActiveEquipe.Id)
        {
            case "ShovelTool":
                if (IsPlayerFartherThanMe(tile)) break;

                Farming.ArarCampo(context, tile);
                break;
            case "PickaxeTool":
                if (IsPlayerFartherThanMe(tile)) break;

                Farming.ArarCampo(context, tile);
                break;
            case "AxeTool":
                break;
            case "SwordTool":
                break;
            case "":
                break;

            default:
                DefaultEntity(tile, Map);
                break;
        }

    }

    private void DigTile(Point tile, TiledMap Map)
    {
        if (IsPlayerFartherThanMe(tile)) return;

        string layer = "Ground";
        bool diggable = Map.GetTilePropertyBool(layer, tile, "diggable");
        string drop = Map.GetTilePropertyString(layer, tile, "drop");

        if (!IsDiggable(TerrainType) || !diggable) return;


        string overrideKey = TerrainType switch
        {
            "Grass" => "DugGrass",
            "Sand" => "DugSand",
            "Dirt" => "DugDirt",
            _ => null
        };
        if (overrideKey == null) return;

        entityWorld.Add(new CampoArado(context, new Vector2(
            tile.X * 16,
            tile.Y * 16
        )));

        // dropar item no meio do Tile.
        int tileSize = 16;
        Vector2 position = new(
            tile.X * tileSize + tileSize / 2,
            tile.Y * tileSize + tileSize / 2
        );
        if (drop != null) DropItem(drop, 3, position);
    }

    private void DefaultEntity(Point tile, TiledMap Map)
    {
        if (IsPlayerTouchingTile(tile)) return;
        // descobrir se o chão pode receber a entidade

        // descobrir se a entidade é do tipo spawned
        Entity entidade = context.State.ActiveEquipe;
        if (!entidade.IsSpawnavel) return;
        string entidadeId = entidade.Id;

        // buscar se a entidade possui quantidade na bag
        int index = context.State.Inventory.PossuiItem(entidadeId);
        if (index < 0) return;

        // invocar entidade no local clicado (Point tile)
        entityWorld.Add(EntityFactory.Create(context, new TiledObjectData
        {
            Type = entidadeId,
            X = tile.X * 16,
            Y = tile.Y * 16
        }));
        // remover a quantidade spawnada da bag
        context.State.Inventory.RemoveItem(entidadeId, 1);
    }

    protected bool IsPlayerTouchingTile(Point tile)
    {
        const int tileSize = 16;

        if (context.State.Player == null) return false;

        Rectangle tileRect = new(
            tile.X * tileSize,
            tile.Y * tileSize,
            tileSize,
            tileSize
        );

        return context.State.Player.Hitbox.Intersects(tileRect);
    }

    public void ChangeEntity(Point tile, Entity beforeEntity, string afterEntity)
    {
        DestroyEntity(beforeEntity);
        Entity item = EntityFactory.Create(context, new TiledObjectData { Type = afterEntity, X = tile.X * 16, Y = tile.Y * 16 });
        entityWorld.Add(item);

        // dropar item no meio do Tile.
        int tileSize = 16;
        Vector2 position = new(
            tile.X * tileSize + tileSize / 2,
            tile.Y * tileSize + tileSize / 2
        );


        if (afterEntity == "Soil02") DropItem("Dirt", 2, position);
        else if (afterEntity == "Soil03") DropItem("Dirt", 1, position);
    }

    public void DropItem(string itemId, int quantidade, Vector2 position)
    {
        if (string.IsNullOrWhiteSpace(itemId) || quantidade <= 0)
            return;

        string saveId = $"{mapId}:drop:{Guid.NewGuid()}";
        Entity item = EntityFactory.Create(context, new TiledObjectData { Type = itemId, X = position.X, Y = position.Y });

        if (item == null)
            return;

        item.SaveId = saveId;
        item.Persistente = true;

        entityWorld.Add(item);
        context.State.AddDroppedItem(mapId, saveId, itemId, quantidade, position);
    }

    public void PickupItem(Entity item)
    {
        if (item == null || !item.IsColetavel)
            return;

        context.State.AddItemToBag(item.Id, 1);
        context.State.RemoveDroppedItem(mapId, item.SaveId);

        entityWorld.Remove(item);
    }

    public void LoadDroppedItems()
    {
        MapSave mapSave = context.State.PlayerSave.GetMapSave(mapId);

        foreach (WorldItemSave savedItem in mapSave.DroppedItems)
        {
            if (!int.TryParse(savedItem.ItemId, out int itemId))
                continue;

            Entity item = EntityFactory.Create(context, new TiledObjectData { Id = itemId, X = savedItem.X, Y = savedItem.Y });

            if (item == null)
                continue;

            item.SaveId = savedItem.SaveId;
            item.Persistente = true;

            entityWorld.Add(item);
        }
    }
    public void LoadEntityMap()
    {
        MapSave mapSave = context.State.PlayerSave.GetMapSave(context.State.PlayerSave.CurrentMap);

        foreach (WorldEntitySave saveEntity in mapSave.SpawnedEntities)
        {
            if (saveEntity.EntityId == "")
                continue;

            Entity item = EntityFactory.Create(context, new TiledObjectData { Type = saveEntity.EntityId, X = saveEntity.X, Y = saveEntity.Y });

            if (item == null)
                continue;

            item.SaveId = saveEntity.SaveId;
            item.Persistente = true;
            if (item is Bau bau)
                bau.AlimentarBau(saveEntity.Data);

            entityWorld.Add(item);
        }
    }

    public bool IsPlayerFartherThanMe(Point point)
    {
        int tileSize = 16;
        int maxTiles = 1;

        Point playerTile = new(
            context.State.Player.Hitbox.Center.X / tileSize,
            context.State.Player.Hitbox.Center.Y / tileSize
        );

        int distanceX = Math.Abs(point.X - playerTile.X);
        int distanceY = Math.Abs(point.Y - playerTile.Y);

        return distanceX > maxTiles || distanceY > maxTiles;
    }
}