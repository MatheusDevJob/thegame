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
    private Texture2D _cursorTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            HardwareModeSwitch = false,
            IsFullScreen = true
        };

        Content.RootDirectory = "Content";
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
        _context.Game.Window.Title = "THE GAME";

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _cursorTexture = Content.Load<Texture2D>("UI/cursor/cursor_03");
        IsMouseVisible = false;

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
        _spriteBatch.Begin();
        DrawCursor(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawCursor(SpriteBatch spriteBatch)
    {
        MouseState mouse = Mouse.GetState();

        spriteBatch.Draw(
            _cursorTexture,
            new Vector2(mouse.X, mouse.Y),
            null,
            Color.White,
            0f,
            Vector2.Zero,
            1.3f,
            SpriteEffects.None,
            0f
        );
    }
}