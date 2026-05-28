using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities;

namespace thegame.Maps;

public abstract class BaseMap : IMap
{
    protected readonly GameContext Context;
    protected readonly TiledMap Map;
    protected readonly EntityWorld EntityWorld = new();

    protected virtual string[] LayersBeforeEntities => ["Ground", "Back"];
    protected virtual string[] LayersAfterEntities => ["Front"];

    public int PixelWidth => Map.PixelWidth;
    public int PixelHeight => Map.PixelHeight;
    public string Id { get; }

    protected BaseMap(GameContext context, string id, string mapPath)
    {
        Context = context;
        Id = id;
        Map = new TiledMap();
        Map.Load(Context.Content, mapPath);
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
        UpdateMap(gameTime);
        EntityWorld.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Map.DrawLayers(spriteBatch, LayersBeforeEntities);
        EntityWorld.Draw(spriteBatch, Context.State.Player);
        Map.DrawLayers(spriteBatch, LayersAfterEntities);
        DrawObjects(spriteBatch);
    }

    public bool Collides(Rectangle hitbox)
    {
        return Map.Collides(hitbox) || EntityWorld.Collides(hitbox);
    }

    protected virtual void UpdateMap(GameTime gameTime)
    {
    }

    protected virtual void DrawObjects(SpriteBatch spriteBatch)
    {
    }
}