using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using thegame.Scenes;
using thegame.UI;

namespace thegame.Core;

public class GameContext(Game game, ContentManager content, GraphicsDevice graphicsDevice, SceneManager sceneManager, InputManager input)
{
    public Game Game { get; } = game;
    public ContentManager Content { get; } = content;
    public GraphicsDevice GraphicsDevice { get; } = graphicsDevice;
    public SceneManager SceneManager { get; } = sceneManager;
    public GameState State { get; set; }
    public UiRenderer UI { get; private set; }
    public InputManager Input { get; } = input;
    public TileCursor TileCursor { get; private set; }
    public UIState UIState { get; } = new();
    public Mercado Mercado { get; } = new();
    public void LoadContent()
    {
        UI = new UiRenderer(this);
        TileCursor = new TileCursor(this);
    }
}