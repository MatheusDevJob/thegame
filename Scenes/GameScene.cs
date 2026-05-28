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
    private readonly Hud _hud;

    public GameScene(GameContext context)
    {
        _context = context;
        _player = _context.State.Player;
        _hud = new Hud(_context);
        _currentMap = new CityMap(_context);
        _camera = new();

        _currentMap.OnEnter();
    }

    public void Update(GameTime gameTime)
    {
        _currentMap.Update(gameTime);
        _player.Update(gameTime, _currentMap.Collides);
        _camera.Follow(
            _player.Center,
            _context.GraphicsDevice.Viewport,
            _currentMap.PixelWidth,
            _currentMap.PixelHeight
        );
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _context.GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.GetTransform(_context.GraphicsDevice.Viewport)
        );
        _currentMap.Draw(spriteBatch);
        spriteBatch.End();




        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        // HUD fixa da tela futuramente
        // Exemplo: vida, dinheiro, inventário, menu rápido
        _hud.Draw(spriteBatch);
        spriteBatch.End();
    }

    private void ChangeMap(IMap nextMap)
    {
        _currentMap.OnExit();
        _currentMap = nextMap;
        _currentMap.OnEnter();
    }
}