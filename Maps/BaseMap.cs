using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Items;
using thegame.Entities.Tools;

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

    protected TileCursor _tileCursor;
    protected InputManager inputManager;
    protected bool isKeyPressed;

    public DebugVisual debugVisual;
    public List<string> logs = [];

    protected BaseMap(GameContext context, string id, string mapPath)
    {
        Context = context;
        Id = id;
        Map = new TiledMap();
        Map.Load(Context.Content, mapPath);

        inputManager = new();
        debugVisual = new(context);
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void Update(GameTime gameTime, TileCursor tileCursor)
    {
        _tileCursor = tileCursor;
        inputManager.Update();
        isKeyPressed = inputManager.IsLeftClickPressed();

        if (isKeyPressed)
        {
            if (IsClickFartherThanPlayer(tileCursor.TilePosition))
            {
                logs.Add("Entidade muito longe.");
                return;
            }

            Entity entity = EntityUnderMouse;
            logs.Add("\n");

            if (entity != null)
            {
                logs.Add($"Clicou na entidade: {entity.GetType().Name}");
                logs.Add($"Mouse World: {_tileCursor.WorldPosition}");
                logs.Add($"Hitbox: {entity.Hitbox}");
                OnEntityClicked(entity);
            }
            else
            {
                logs.Add($"Clicou no tile: {_tileCursor.TilePosition}");
                logs.Add($"Mouse World: {_tileCursor.WorldPosition}");
                OnTileClicked(_tileCursor.TilePosition);
            }

            if (logs.Count > 6)
                logs.RemoveRange(0, logs.Count - 6);
        }

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

    public Entity EntityUnderMouse => EntityWorld.GetEntityAt(_tileCursor.WorldPosition, Context.State.Player);

    protected virtual void UpdateMap(GameTime gameTime)
    {
    }

    protected virtual void DrawObjects(SpriteBatch spriteBatch)
    {
    }

    protected virtual void OnEntityClicked(Entity entity)
    {
        switch (entity)
        {
            case Tronco tronco when Context.State.Player.ActiveTool is AxeTool tool:
                if (Context.State.Player.IsAnimated)
                    return;

                Context.State.Player.PlayActionAnimation(10, 8, () =>
                {
                    tronco.Life -= tool.Damage;

                    logs.Add($"Machadada no tronco! Vida restante: {tronco.Life}");

                    if (tronco.Life <= 0)
                        EntityWorld.Remove(tronco);
                });

                break;

            default:
                return;
        }
    }

    protected virtual void OnTileClicked(Point tile)
    {
    }
    protected bool IsClickFartherThanPlayer(Point clickedTile, int maxTiles = 2)
    {
        int tileSize = 16;

        Point playerTile = new(
            Context.State.Player.Hitbox.Center.X / tileSize,
            Context.State.Player.Hitbox.Center.Y / tileSize
        );

        int distanceX = Math.Abs(clickedTile.X - playerTile.X);
        int distanceY = Math.Abs(clickedTile.Y - playerTile.Y);

        return distanceX > maxTiles || distanceY > maxTiles;
    }

    public virtual void DrawDebug(SpriteBatch spriteBatch)
    {
        debugVisual.Draw(spriteBatch, logs);
    }
}