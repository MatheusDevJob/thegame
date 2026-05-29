using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;

namespace thegame.Scenes;

public class MainMenuScene : IScene
{
    private readonly GameContext _context;
    private readonly SpriteFont _font;
    private readonly Texture2D _pixel;
    private MouseState _previousMouse;

    private Rectangle _startButton;
    private Rectangle _exitButton;

    public MainMenuScene(GameContext context)
    {
        _context = context;

        // Assets do menu
        _font = _context.Content.Load<SpriteFont>("Fonts/MenuFont");

        _pixel = new Texture2D(_context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        int buttonWidth = 280;
        int buttonHeight = 55;
        int x = (_context.GraphicsDevice.Viewport.Width - buttonWidth) / 2;
        int y = (_context.GraphicsDevice.Viewport.Height / 2) - 20;

        _startButton = new Rectangle(x, y, buttonWidth, buttonHeight);
        _exitButton = new Rectangle(x, y + 70, buttonWidth, buttonHeight);
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouse = Mouse.GetState();

        if (WasClicked(mouse, _startButton))
        {
            // Aqui começa o jogo.
            // Se quiser começar um jogo novo, pode criar World/TimeSystem aqui.


            // Troca para a cena do jogo.
            // A GameScene recebe o mesmo GameContext, então acessa o mesmo World, TimeSystem, Content, etc.
            _context.State = new GameState(_context, new GameSave
            {
                PlayerLife = 75f,
                PlayerPosition = new Vector2(100, 230)
            });
            _context.SceneManager.ChangeScene(new GameScene(_context));
        }

        if (WasClicked(mouse, _exitButton))
        {
            _context.Game.Exit();
        }

        _previousMouse = mouse;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int width = _context.GraphicsDevice.Viewport.Width;
        int height = _context.GraphicsDevice.Viewport.Height;

        _context.GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        spriteBatch.Draw(_pixel, new Rectangle(0, 0, width, height), new Color(18, 20, 28));

        DrawCenteredText(spriteBatch, "THE GAME", 120, Color.White, 2f);

        DrawButton(spriteBatch, _startButton, "Iniciar");
        DrawButton(spriteBatch, _exitButton, "Sair");
        spriteBatch.End();
    }

    private bool WasClicked(MouseState mouse, Rectangle rectangle)
    {
        return rectangle.Contains(mouse.Position) &&
               mouse.LeftButton == ButtonState.Pressed &&
               _previousMouse.LeftButton == ButtonState.Released;
    }

    private void DrawButton(SpriteBatch spriteBatch, Rectangle rectangle, string text)
    {
        bool hovering = rectangle.Contains(Mouse.GetState().Position);

        Color background = hovering ? new Color(75, 85, 120) : new Color(45, 50, 70);

        spriteBatch.Draw(_pixel, rectangle, background);

        Vector2 textSize = _font.MeasureString(text);
        Vector2 position = new(
            rectangle.X + (rectangle.Width - textSize.X) / 2,
            rectangle.Y + (rectangle.Height - textSize.Y) / 2
        );

        spriteBatch.DrawString(_font, text, position, Color.White);
    }

    private void DrawCenteredText(SpriteBatch spriteBatch, string text, float y, Color color, float scale = 1f)
    {
        Vector2 size = _font.MeasureString(text) * scale;
        Vector2 position = new((_context.GraphicsDevice.Viewport.Width - size.X) / 2, y);

        spriteBatch.DrawString(_font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}