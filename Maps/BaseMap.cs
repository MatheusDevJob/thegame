using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Npcs;

namespace thegame.Maps;

public abstract class BaseMap : IMap
{
    protected readonly GameContext Context;
    protected readonly TiledMap Map;
    protected readonly EntityWorld EntityWorld = new();
    protected readonly WorldActionService _worldActionService;
    protected readonly EntityInteractionManager _entityInteractionManager;

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

        inputManager = context.Input;
        _worldActionService = new(context, EntityWorld, id);
        _entityInteractionManager = new(context, _worldActionService);
        debugVisual = new(context);

        Map = new TiledMap();
        Map.Load(Context.Content, mapPath);
    }

    public virtual void OnEnter()
    {
        EntityWorld.ClearAll();

        EntityWorld.Add(new Aldeao(
            Context,
            "Aldeão",
            "Olá, viajante!",
            new Vector2(1100, 205)
        ));

        foreach (var obj in Map.GetObjects("Objects"))
        {
            Entity entity = EntityFactory.Create(Context, obj);

            if (entity == null)
                continue;

            entity.SaveId = $"{Id}:{obj.Id}";

            if (Context.State.IsEntityRemoved(Id, entity.SaveId))
                continue;

            EntityWorld.Add(entity);
        }

        _worldActionService.LoadDroppedItems();
    }

    public virtual void OnExit()
    {
        // EntityWorld.clearAll();
    }

    public virtual void Update(GameTime gameTime, TileCursor tileCursor)
    {
        _tileCursor = tileCursor;
        isKeyPressed = inputManager.IsLeftClickPressed();
        GameState State = Context.State;

        if (isKeyPressed && !State.LayoutMenu && !State.LayoutBag)
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
                logs.Add($"Entity Id: {entity.Id}");
                logs.Add($"SaveId: {entity.SaveId}");
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

        DrawDebug(spriteBatch);
    }

    public bool Collides(Rectangle hitbox)
    {
        return Map.Collides(hitbox) || EntityWorld.Collides(hitbox);
    }

    public Entity EntityUnderMouse => EntityWorld.GetEntityAt(_tileCursor.WorldPosition, Context.State.Player);

    protected virtual void UpdateMap(GameTime gameTime)
    {
        Player player = Context.State.Player;
        Entity item = EntityWorld.GetCollectableIntersecting(player.Hitbox, player);

        if (item != null)
            _worldActionService.PickupItem(item);
    }

    protected virtual void DrawObjects(SpriteBatch spriteBatch)
    {
    }

    protected virtual void OnEntityClicked(Entity entity)
    {
        _entityInteractionManager.HandleClick(entity);
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