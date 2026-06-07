using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.WorldObjects.Solo;

namespace thegame.Maps;

public class WorldActionService(GameContext context, EntityWorld entityWorld, string mapId)
{
    private readonly GameContext _context = context;
    private readonly EntityWorld _entityWorld = entityWorld;
    private readonly string _mapId = mapId;
    private static readonly HashSet<string> DiggableTerrains = new(StringComparer.OrdinalIgnoreCase)
        {
            "Grass",
            "Sand",
            "Dirt"
        };

    public bool IsDiggable(string terrainType)
    {
        return !string.IsNullOrEmpty(terrainType) && DiggableTerrains.Contains(terrainType) && _context.State.ActiveEquipe.Id == "ShovelTool";
    }

    public void DestroyEntity(Entity entity)
    {
        if (entity == null)
            return;

        if (entity.Persistente)
            _context.State.MarkEntityRemoved(_mapId, entity.SaveId);

        _entityWorld.Remove(entity);
    }

    public void DigTile(Point tile, TiledMap Map)
    {
        if (IsPlayerFartherThanMe(tile)) return;

        string layer = "Ground";
        string terrainType = Map.GetTilePropertyString(layer, tile, "terrainType");
        bool diggable = Map.GetTilePropertyBool(layer, tile, "diggable");
        string drop = Map.GetTilePropertyString(layer, tile, "drop");


        if (string.IsNullOrEmpty(terrainType) || !diggable) return;
        if (!IsDiggable(terrainType)) return;


        string overrideKey = terrainType switch
        {
            "Grass" => "DugGrass",
            "Sand" => "DugSand",
            "Dirt" => "DugDirt",
            _ => null
        };
        if (overrideKey == null) return;

        _entityWorld.Add(new Soil01(_context, new Vector2(
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

    public void ChangeEntity(Point tile, Entity beforeEntity, string afterEntity)
    {
        DestroyEntity(beforeEntity);
        Entity item = EntityFactory.Create(_context, new TiledObjectData { Type = afterEntity, X = tile.X * 16, Y = tile.Y * 16 });
        _entityWorld.Add(item);

        // dropar item no meio do Tile.
        int tileSize = 16;
        Vector2 position = new(
            tile.X * tileSize + tileSize / 2,
            tile.Y * tileSize + tileSize / 2
        );


        if (afterEntity == "Soil02") DropItem("Dirt", 2, position);
        else if (afterEntity == "Soil03") DropItem("Dirt", 1, position);
    }

    public void DropItem(string itemId, int amount, Vector2 position)
    {
        if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
            return;

        string saveId = $"{_mapId}:drop:{Guid.NewGuid()}";
        Entity item = EntityFactory.Create(_context, new TiledObjectData { Type = itemId, X = position.X, Y = position.Y });

        if (item == null)
            return;

        item.SaveId = saveId;
        item.Persistente = true;

        _entityWorld.Add(item);
        _context.State.AddDroppedItem(_mapId, saveId, itemId, amount, position);
    }

    public void PickupItem(Entity item)
    {
        if (item == null || !item.IsColetavel)
            return;

        _context.State.AddItemToBag(item.Id, 1);
        _context.State.RemoveDroppedItem(_mapId, item.SaveId);

        _entityWorld.Remove(item);
    }

    public void LoadDroppedItems()
    {
        MapSave mapSave = _context.State.PlayerSave.GetMapSave(_mapId);

        foreach (WorldItemSave savedItem in mapSave.DroppedItems)
        {
            if (!int.TryParse(savedItem.ItemId, out int itemId))
                continue;

            Entity item = EntityFactory.Create(_context, new TiledObjectData { Id = itemId, X = savedItem.X, Y = savedItem.Y });

            if (item == null)
                continue;

            item.SaveId = savedItem.SaveId;
            item.Persistente = true;

            _entityWorld.Add(item);
        }
    }
    public void LoadEntityMap()
    {
        MapSave mapSave = _context.State.PlayerSave.GetMapSave(_context.State.PlayerSave.CurrentMap);

        foreach (WorldEntitySave saveEntity in mapSave.SpawnedEntities)
        {
            if (saveEntity.EntityId == "")
                continue;

            Entity item = EntityFactory.Create(_context, new TiledObjectData { Type = saveEntity.EntityId, X = saveEntity.X, Y = saveEntity.Y });

            if (item == null)
                continue;

            item.SaveId = saveEntity.SaveId;
            item.Persistente = true;

            _entityWorld.Add(item);
        }
    }

    public bool IsPlayerFartherThanMe(Point point)
    {
        int tileSize = 16;
        int maxTiles = 1;

        Point playerTile = new(
            _context.State.Player.Hitbox.Center.X / tileSize,
            _context.State.Player.Hitbox.Center.Y / tileSize
        );

        int distanceX = Math.Abs(point.X - playerTile.X);
        int distanceY = Math.Abs(point.Y - playerTile.Y);

        return distanceX > maxTiles || distanceY > maxTiles;
    }
}