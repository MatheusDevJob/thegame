using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Entities;

namespace thegame.Maps;

public abstract class BaseMap : IMap
{
    protected readonly GameContext Context;
    protected readonly TiledMap Map;
    protected readonly EntityWorld EntityWorld;
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
        context.State.MapaAtualId = id;
        Id = id;
        EntityWorld = context.State.EntityWorld;

        inputManager = context.Input;
        _worldActionService = new(context, EntityWorld, id);
        _entityInteractionManager = new(context, _worldActionService);
        // debugVisual = new(context);

        Map = new TiledMap();
        Context.State.TiledMap = Map;
        Map.Load(Context.Content, mapPath);
    }

    public virtual void OnEnter()
    {
        _worldActionService.LoadDroppedItems();
        _worldActionService.LoadEntityMap();
    }

    public virtual void OnExit()
    {
        EntityWorld.ClearAll();
    }

    public virtual void Update(GameTime gameTime, TileCursor tileCursor)
    {
        _tileCursor = tileCursor;
        isKeyPressed = inputManager.IsLeftClickPress();
        GameState State = Context.State;
        Point PosicaoMouse = _tileCursor.TilePosition;
        if (_worldActionService.IsPlayerFartherThanMe(PosicaoMouse))
        {
            // no futuro fazer o boneco xingar igual nordestino
        }
        else if (isKeyPressed && !State.LayoutMenu && !State.LayoutBag)
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
                    _entityInteractionManager.HandleClick(entity, PosicaoMouse, "left");

                }
                else
                {
                    // logs.Add($"Clicou no tile: {_tileCursor.TilePosition}");
                    // logs.Add($"Mouse World: {_tileCursor.WorldPosition}");
                    _worldActionService.OnTileClicked(PosicaoMouse, Map);
                }

                if (!isKeyPressed)
                {
                    Context.State.Player.ResetarAnimacao();
                }
            });

            // if (logs.Count > 6)
            // // logs.RemoveRange(0, logs.Count - 6);
        }

        IsM2Clicked(gameTime, PosicaoMouse);

        UpdateMap(gameTime);
        EntityWorld.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Map.DrawLayers(spriteBatch, LayersBeforeEntities);
        EntityWorld.Draw(spriteBatch, Context.State.Player);
        Map.DrawLayers(spriteBatch, LayersAfterEntities);
        // DrawDebug(spriteBatch);

        if (_noEvento)
            DrawEventoIsPerto(spriteBatch);
    }

    private Vector2 PosicaoBotaoEventoTela;
    private void DrawEventoIsPerto(SpriteBatch spriteBatch)
    {
        Vector2 position = new(PosicaoBotaoEventoTela.X, PosicaoBotaoEventoTela.Y - 16);
        Context.UI.DrawKeyHint(spriteBatch, "E", position);
    }

    private bool _noEvento = false;
    public void CheckEventos(Rectangle hitbox)
    {
        Dictionary<string, dynamic> Evento = Map.OnEventCollides(hitbox);
        if (Evento != null && !_noEvento)
        {
            _noEvento = true;
            PosicaoBotaoEventoTela = Evento["Position"];
            return;
        }

        if (Evento == null)
            _noEvento = false;

        if (_noEvento && Context.Input.IsKeyPressed(Keys.E))
        {
            List<TiledPropertyData> listaDatas = Evento["Properties"];
            TiledPropertyData data = listaDatas.FirstOrDefault((e) => e.Type == "string" && e.Name == "portal");
            string a = data.Value.ToString();
            bool r = Context.State.StartEvent(a);
            if (r)
            {
                Context.State.SetPosicaoPlayerMapa(a);
            }
        }
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

    protected virtual void IsM2Clicked(GameTime gameTime, Point PosicaoMouse)
    {
        isRMPressed = inputManager.IsRightClickPressed();
        if (!isRMPressed) return;

        Entity entity = EntityUnderMouse;
        if (entity == null) return;

        _entityInteractionManager.HandleClick(entity, PosicaoMouse, "right");
    }


    public virtual void DrawDebug(SpriteBatch spriteBatch)
    {
        // debugVisual.Draw(spriteBatch, logs);
    }
}