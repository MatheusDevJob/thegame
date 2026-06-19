using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities;
using thegame.Maps;
using thegame.UI;

namespace thegame.Scenes;

public class GameScene : IScene
{
    private readonly GameContext _context;
    private readonly Player _player;
    private IMap _currentMap;
    private readonly Camera2D _camera;
    protected readonly BaseHud _hud;
    private readonly TileCursor _tileCursor;

    public GameScene(GameContext context, IMap currentMap)
    {
        _context = context;
        _player = _context.State.Player;
        _hud = new HudManager(_context);
        _currentMap = currentMap;
        _camera = new();
        _tileCursor = context.TileCursor;

        _currentMap.OnEnter();
    }

    public void Update(GameTime gameTime)
    {
        _tileCursor.Update(_camera);

        _currentMap.Update(gameTime, _tileCursor);
        _player.Update(gameTime, _currentMap.Collides);
        _camera.Follow(
            _player.Center,
            _context.GraphicsDevice.Viewport,
            _currentMap.PixelWidth,
            _currentMap.PixelHeight
        );
        _hud.Update(gameTime);

        _currentMap.CheckEventos(_player.Hitbox);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _context.GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.GetTransform(_context.GraphicsDevice.Viewport)
        );
        _currentMap.Draw(spriteBatch);
        _tileCursor.Draw(spriteBatch);
        spriteBatch.End();




        spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            blendState: BlendState.AlphaBlend
        );
        // HUD fixa da tela futuramente
        // Exemplo: vida, dinheiro, inventário, menu rápido
        _hud.Draw(spriteBatch);
        _currentMap.DrawDebug(spriteBatch);
        spriteBatch.End();
    }

    public void ChangeMap(BaseMap nextMap)
    {
        // primeiro salva
        _context.State.SyncSaveFromRuntime();

        // segundo limpa
        _currentMap.OnExit();

        // terceiro troca o mapa e o id atual
        _currentMap = nextMap;
        _context.State.PlayerSave.CurrentMap = nextMap.Id;

        _currentMap.OnEnter();
    }
}