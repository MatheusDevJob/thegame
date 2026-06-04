using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Items.WorldObjects.Interactables;

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


        MapSave mapSave = _context.State.PlayerSave.GetMapSave(_mapId);

        mapSave.SetTileState(new WorldTileSave
        {
            Layer = layer,
            X = tile.X,
            Y = tile.Y,
            BaseTerrain = terrainType,
            State = "Dug",
            OverrideKey = overrideKey
        });

        Map.SetTileOverride(layer, tile, overrideKey);

        // dropar item no meio do Tile.
        int tileSize = 16;
        Vector2 position = new(
            tile.X * tileSize + tileSize / 2,
            tile.Y * tileSize + tileSize / 2
        );
        if (drop != null) DropItem(drop, 3, position);
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
}