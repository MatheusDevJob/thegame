using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.UI;

public class UiRenderer
{
    private readonly SpriteFont _font;
    private readonly Texture2D _pixel;
    private readonly Texture2D _buttonsTile;
    private readonly Texture2D _dialogTile;
    private readonly Color _textColor;

    public UiRenderer(GameContext context)
    {
        _font = context.Content.Load<SpriteFont>("Fonts/MenuFont");
        _pixel = new Texture2D(context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _textColor = new Color(144, 98, 96);

        _buttonsTile = context.Content.Load<Texture2D>("Sprout Lands - UI Pack - Basic pack/Sprite sheets/buttons/Square Buttons 26x26");
        _dialogTile = context.Content.Load<Texture2D>("Sprout Lands - UI Pack - Basic pack/Sprite sheets/Dialouge UI/dialog box big");
    }

    public void DrawKeyHint(SpriteBatch spriteBatch, string text, Vector2 position)
    {
        float scale = 0.3f;
        string fullText = $"{text}";
        Vector2 textSize = _font.MeasureString(fullText) * scale;

        Rectangle source = new(59, 59, 26, 26);

        Rectangle background = new(
            (int)(position.X - 13),
            (int)(position.Y - 13),
            26,
            26
        );

        spriteBatch.Draw(_buttonsTile, background, source, Color.White);

        Vector2 textPosition = new(
            background.X + background.Width / 2f - textSize.X / 2f,
            background.Y + background.Height / 2f - textSize.Y / 2f
        );

        spriteBatch.DrawString(
            _font,
            fullText,
            textPosition,
            _textColor,
            0f,
            Vector2.Zero,
            scale,
            SpriteEffects.None,
            0f
        );
    }

    public void DrawNpcSpeech(SpriteBatch spriteBatch, string text, Vector2 targetCenter, float targetTopY)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        float scale = 0.42f;
        int paddingX = 30;
        int paddingY = 18;
        int offsetY = 26;

        string fullText = text;
        Vector2 textSize = _font.MeasureString(fullText) * scale;

        int minWidth = 140;
        int minHeight = 36;

        int boxWidth = Math.Max((int)(textSize.X + paddingX * 2), minWidth);
        int boxHeight = Math.Max((int)(textSize.Y + paddingY * 2), minHeight);

        Rectangle background = new(
            (int)(targetCenter.X - boxWidth / 2f),
            (int)(targetTopY - boxHeight - offsetY),
            boxWidth,
            boxHeight
        );

        spriteBatch.Draw(_dialogTile, background, Color.White);

        Vector2 textPosition = new(
            background.X + background.Width / 2f - textSize.X / 2f,
            background.Y + background.Height / 2f - textSize.Y / 2f
        );

        spriteBatch.DrawString(_font, fullText, textPosition, _textColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}