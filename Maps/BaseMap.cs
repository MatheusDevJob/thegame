using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Npcs;
using thegame.Entities.Tools;

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
    protected bool isRMPressed;

    // public DebugVisual debugVisual;
    public List<string> logs = [];

    protected BaseMap(GameContext context, string id, string mapPath)
    {
        Context = context;
        Id = id;

        inputManager = context.Input;
        _worldActionService = new(context, EntityWorld, id);
        _entityInteractionManager = new(context, _worldActionService);
        // debugVisual = new(context);

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
            Entity entity = EntityUnderMouse;
            // logs.Add("\n"); 
            State.Player.PlayActionAnimation(State.ActiveEquipe.Id, () =>
            {
                if (entity != null)
                {
                    // logs.Add($"Clicou na entidade: {entity.GetType().Name}");
                    // logs.Add($"Entity Id: {entity.Id}");
                    // logs.Add($"SaveId: {entity.SaveId}");
                    // logs.Add($"Hitbox: {entity.Hitbox}");
                    OnEntityClicked(entity);
                }
                else
                {
                    // logs.Add($"Clicou no tile: {_tileCursor.TilePosition}");
                    // logs.Add($"Mouse World: {_tileCursor.WorldPosition}");
                    OnTileClicked(_tileCursor.TilePosition);
                }
            });

            // if (logs.Count > 6)
            // // logs.RemoveRange(0, logs.Count - 6);
        }
        IsM2Clicked(gameTime);

        UpdateMap(gameTime);
        TrocaTool();
        EntityWorld.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Map.DrawLayers(spriteBatch, LayersBeforeEntities);
        EntityWorld.Draw(spriteBatch, Context.State.Player);
        Map.DrawLayers(spriteBatch, LayersAfterEntities);
        DrawObjects(spriteBatch);

        // DrawDebug(spriteBatch);
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

    protected virtual void IsM2Clicked(GameTime gameTime)
    {
        isRMPressed = inputManager.IsRightClickPressed();
        if (!isRMPressed) return;

        Entity entity = EntityUnderMouse;
        if (entity == null) return;

        OnEntityClicked(entity, "right");
    }

    protected virtual void DrawObjects(SpriteBatch spriteBatch)
    {
    }

    protected virtual void OnEntityClicked(Entity entity, string click = "left")
    {
        _entityInteractionManager.HandleClick(entity, click);
    }

    protected virtual void OnTileClicked(Point tile)
    {
        _worldActionService.DigTile(tile, Map);
    }
    private void TrocaTool()
    {
        InputManager input = Context.Input;
        if (input.IsKeyPressed(Keys.D1))
        {
            Context.State.SetActiveEquipe("");
        }
        else if (input.IsKeyPressed(Keys.D2))
        {
            Context.State.SetActiveEquipe("");
        }
        else if (input.IsKeyPressed(Keys.D3))
        {
            Context.State.SetActiveEquipe("");
        }
        else if (input.IsKeyPressed(Keys.D4))
        {
            Context.State.SetActiveEquipe("");
        }
        else if (input.IsKeyPressed(Keys.D5))
        {
            Context.State.SetActiveEquipe("");
        }
        else if (input.IsKeyPressed(Keys.D6))
        {
            Context.State.SetActiveEquipe("");
        }
        else if (input.IsKeyPressed(Keys.D7))
        {
            Context.State.SetActiveEquipe("");
        }
        else if (input.IsKeyPressed(Keys.D8))
        {
            Context.State.SetActiveEquipe("");
        }
    }
    public virtual void DrawDebug(SpriteBatch spriteBatch)
    {
        // debugVisual.Draw(spriteBatch, logs);
    }
}