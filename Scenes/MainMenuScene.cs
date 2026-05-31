using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.UI;

namespace thegame.Scenes;

public class MainMenuScene : IScene
{
    private readonly GameContext _context;
    private readonly SpriteFont _font;
    private readonly Texture2D _pixel;
    private readonly AnimatedBackground _background;
    private MouseState _previousMouse;

    private Rectangle _startButton;
    private Rectangle _exitButton;

    public MainMenuScene(GameContext context)
    {
        _context = context;
        _font = _context.Content.Load<SpriteFont>("Fonts/MenuFont");

        _pixel = new Texture2D(_context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        Texture2D backgroundTexture = _context.Content.Load<Texture2D>("UI/animate_background");
        _background = new AnimatedBackground(backgroundTexture, 640, 384, 5, 30, 0.08);

        int buttonWidth = 180;
        int buttonHeight = 55;
        int buttonSpacing = 20;

        float titleY = 120f;
        float titleScale = 2f;
        Vector2 titleSize = _font.MeasureString("THE GAME") * titleScale;

        int totalButtonsWidth = buttonWidth * 2 + buttonSpacing;
        int startX = (_context.GraphicsDevice.Viewport.Width - totalButtonsWidth) / 2;
        int buttonY = (int)(titleY + titleSize.Y + 35);

        _startButton = new Rectangle(startX, buttonY, buttonWidth, buttonHeight);
        _exitButton = new Rectangle(startX + buttonWidth + buttonSpacing, buttonY, buttonWidth, buttonHeight);
    }

    public void Update(GameTime gameTime)
    {
        _background.Update(gameTime);

        MouseState mouse = Mouse.GetState();

        if (WasClicked(mouse, _startButton))
        {
            List<string> lista = ["axe"];

            _context.State = new GameState(_context, new GameSave
            {
                PlayerLife = 75f,
                PlayerPosition = new Vector2(1200, 220),
                ListTools = lista,
                ActiveTool = lista[0],
            });

            _context.SceneManager.ChangeScene(new GameScene(_context));
        }

        if (WasClicked(mouse, _exitButton))
            _context.Game.Exit();

        _previousMouse = mouse;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;

        _context.GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _background.Draw(spriteBatch, viewport);

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
        Color background = hovering ? new Color(75, 85, 120, 220) : new Color(45, 50, 70, 200);

        spriteBatch.Draw(_pixel, rectangle, background);

        Vector2 textSize = _font.MeasureString(text);
        Vector2 position = new(
            rectangle.X + (rectangle.Width - textSize.X) / 2,
            rectangle.Y + (rectangle.Height - textSize.Y) / 2
        );

        spriteBatch.DrawString(_font, text, position, Color.White);
    }

    private void DrawCenteredText(SpriteBatch spriteBatch, string text, float y, Color textColor, float scale = 1f)
    {

        Vector2 size = _font.MeasureString(text) * scale;

        int paddingX = 35;
        int paddingY = 18;

        Rectangle background = new(
            (int)((_context.GraphicsDevice.Viewport.Width - size.X) / 2 - paddingX),
            (int)(y - paddingY),
            (int)(size.X + paddingX * 2),
            (int)(size.Y + paddingY * 2)
        );

        spriteBatch.Draw(_pixel, background, new Color(0, 0, 0, 180));

        Vector2 position = new(
            background.X + (background.Width - size.X) / 2,
            background.Y + (background.Height - size.Y) / 2
        );

        spriteBatch.DrawString(_font, text, position, textColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}