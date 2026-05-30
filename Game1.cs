using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Scenes;

namespace thegame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;
    private GameContext _context;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            HardwareModeSwitch = false,
            IsFullScreen = true
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _sceneManager = new SceneManager();

        _context = new GameContext(
            this,
            Content,
            GraphicsDevice,
            _sceneManager,
            new InputManager()
        );

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _context.LoadContent();
        _sceneManager.ChangeScene(new MainMenuScene(_context));
    }

    protected override void Update(GameTime gameTime)
    {
        _context.Input.Update();
        _sceneManager.Update(gameTime);
        if (_context.Input.IsKeyPressed(Keys.Escape))
            Exit();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _sceneManager.Draw(_spriteBatch);
        base.Draw(gameTime);
    }
}