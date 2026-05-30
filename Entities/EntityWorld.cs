using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thegame.Entities;

public class EntityWorld
{
    private readonly List<Entity> _entities = [];

    public IReadOnlyList<Entity> Entities => _entities;

    public void Add(Entity entity)
    {
        if (!_entities.Contains(entity))
            _entities.Add(entity);
    }

    public void Remove(Entity entity)
    {
        _entities.Remove(entity);
    }

    public IEnumerable<T> GetEntities<T>() where T : Entity
    {
        return _entities.OfType<T>();
    }

    public bool Collides(Rectangle hitbox, Entity ignore = null)
    {
        foreach (Entity entity in _entities)
        {
            if (entity == ignore)
                continue;

            if (!entity.BloqueiaMovimento)
                continue;

            if (hitbox.Intersects(entity.Hitbox))
                return true;
        }

        return false;
    }

    public void Update(GameTime gameTime)
    {
        foreach (Entity entity in _entities)
            entity.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, params Entity[] extras)
    {
        foreach (Entity entity in _entities.Concat(extras).Distinct().OrderBy(entity => entity.SortY))
            entity.Draw(spriteBatch);
    }

    public Entity GetEntityAt(Vector2 worldPosition, Entity ignore = null)
    {
        Point point = new((int)worldPosition.X, (int)worldPosition.Y);

        return _entities
            .Where(entity => entity != ignore)
            .Where(entity => entity.Hitbox.Contains(point))
            .OrderByDescending(entity => entity.SortY)
            .FirstOrDefault();
    }
}