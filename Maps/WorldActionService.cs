using System;
using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;

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
        if (!int.TryParse(itemId, out int itemIntId))
            return;
        Entity item = EntityFactory.Create(_context, new TiledObjectData { Id = itemIntId, X = position.X, Y = position.Y });

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