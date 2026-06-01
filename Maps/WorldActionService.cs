using System;
using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Items;

namespace thegame.Maps;

public class WorldActionService(GameContext context, EntityWorld entityWorld, string mapId)
{
    private readonly GameContext _context = context;
    private readonly EntityWorld _entityWorld = entityWorld;
    private readonly string _mapId = mapId;

    public void DestroyEntity(Entity entity)
    {
        if (entity == null)
            return;

        if (entity.Persistente)
            _context.State.MarkEntityRemoved(_mapId, entity.SaveId);

        _entityWorld.Remove(entity);
    }

    public void DropItem(string itemId, int amount, Vector2 position)
    {
        if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
            return;

        string saveId = $"{_mapId}:drop:{Guid.NewGuid()}";
        Entity item = CreateItemEntity(itemId, position);

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
            Entity item = CreateItemEntity(savedItem.ItemId, new Vector2(savedItem.X, savedItem.Y));

            if (item == null)
                continue;

            item.SaveId = savedItem.SaveId;
            item.Persistente = true;

            _entityWorld.Add(item);
        }
    }

    private Entity CreateItemEntity(string itemId, Vector2 position)
    {
        return itemId switch
        {
            "Wood" => new Wood(_context, position),
            _ => null
        };
    }
}