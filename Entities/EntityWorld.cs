using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Entities.Decoracoes;

namespace thegame.Entities;

public class EntityWorld
{
    public readonly List<Entity> _entities = [];

    public IReadOnlyList<Entity> Entities => _entities;

    public void Add(Entity entity)
    {
        if (entity == null || _entities.Contains(entity))
            return;

        _entities.Add(entity);

        if (entity is Cerca cerca)
            AtualizarCercaEVizinhas(cerca.TilePosition);
    }

    public void Remove(Entity entity)
    {
        if (entity == null)
            return;

        Point? cercaTile = null;

        if (entity is Cerca cerca)
            cercaTile = cerca.TilePosition;

        _entities.Remove(entity);

        if (cercaTile.HasValue)
            AtualizarCercaEVizinhas(cercaTile.Value);
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
        foreach (Entity entity in _entities.ToArray())
            entity.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Player player)
    {
        foreach (var entity in _entities.Where(e => e.RenderLayer == EntityRenderLayer.Ground))
            entity.Draw(spriteBatch);

        var normalEntities = _entities
            .Where(e => e.RenderLayer == EntityRenderLayer.Normal)
            .Append(player)
            .OrderBy(e => e.SortY);

        foreach (var entity in normalEntities)
            entity.Draw(spriteBatch);

        foreach (var entity in _entities.Where(e => e.RenderLayer == EntityRenderLayer.Top))
            entity.Draw(spriteBatch);
    }

    public Entity GetEntityAt(Vector2 worldPosition, Entity ignore = null)
    {
        Point point = new((int)worldPosition.X, (int)worldPosition.Y);

        return _entities
            .Where(entity => entity != ignore)
            .Where(entity => entity.Hitbox.Contains(point))
            .OrderByDescending(entity => (int)entity.RenderLayer)
            .ThenByDescending(entity => entity.SortY)
            .FirstOrDefault();
    }

    public Entity GetCollectableIntersecting(Rectangle hitbox, Entity ignore = null)
    {
        return _entities
            .Where(entity => entity != ignore)
            .Where(entity => entity.IsColetavel)
            .Where(entity => hitbox.Intersects(entity.Hitbox))
            .OrderByDescending(entity => entity.SortY)
            .FirstOrDefault();
    }

    public Entity IntersectsAny(Rectangle hitbox, Entity ignore = null)
    {
        foreach (Entity entity in _entities)
        {
            if (entity == ignore)
                continue;

            if (hitbox.Intersects(entity.Hitbox))
                return entity;
        }

        return null;
    }

    public void ClearAll()
    {
        _entities.Clear();
    }

    public Cerca GetCercaAt(Point tile, Entity ignore = null)
    {
        foreach (Cerca cerca in GetEntities<Cerca>())
        {
            if (cerca == ignore)
                continue;

            if (cerca.TilePosition == tile)
                return cerca;
        }

        return null;
    }

    public bool HasCercaAt(Point tile, Entity ignore = null)
    {
        return GetCercaAt(tile, ignore) != null;
    }

    private void AtualizarCercaEVizinhas(Point tile)
    {
        AtualizarCercaNoTile(tile);
        AtualizarCercaNoTile(new Point(tile.X, tile.Y - 1));
        AtualizarCercaNoTile(new Point(tile.X + 1, tile.Y));
        AtualizarCercaNoTile(new Point(tile.X, tile.Y + 1));
        AtualizarCercaNoTile(new Point(tile.X - 1, tile.Y));
    }

    private void AtualizarCercaNoTile(Point tile)
    {
        Cerca cerca = GetCercaAt(tile);

        if (cerca == null)
            return;

        cerca.AtualizarSprite(this);
    }

    public void AtualizarTodasAsCercas()
    {
        foreach (Cerca cerca in GetEntities<Cerca>())
            cerca.AtualizarSprite(this);
    }
}