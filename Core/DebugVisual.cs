using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thegame.Core;

public class DebugVisual
{
    private readonly Texture2D _pixel;
    private readonly SpriteFont _font;
    private readonly Rectangle _baseLog;

    public DebugVisual(GameContext context)
    {
        _pixel = new Texture2D(context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _font = context.Content.Load<SpriteFont>("Fonts/MenuFont");

        _baseLog = new Rectangle(20, 20, 520, 240);
    }

    public void Draw(SpriteBatch spriteBatch, List<string> logs)
    {
        spriteBatch.Draw(_pixel, _baseLog, new Color(0, 0, 0, 190));

        spriteBatch.DrawString(
            _font,
            "DEBUG",
            new Vector2(_baseLog.X + 12, _baseLog.Y + 10),
            Color.Yellow,
            0f,
            Vector2.Zero,
            0.8f,
            SpriteEffects.None,
            0f
        );

        if (logs == null || logs.Count == 0)
        {
            spriteBatch.DrawString(
                _font,
                "Sem logs ainda...",
                new Vector2(_baseLog.X + 12, _baseLog.Y + 42),
                Color.White,
                0f,
                Vector2.Zero,
                0.65f,
                SpriteEffects.None,
                0f
            );

            return;
        }

        int maxLines = 7;
        int start = logs.Count > maxLines ? logs.Count - maxLines : 0;

        for (int i = start; i < logs.Count; i++)
        {
            Vector2 position = new(
                _baseLog.X + 12,
                _baseLog.Y + 42 + ((i - start) * 24)
            );

            spriteBatch.DrawString(
                _font,
                logs[i],
                position,
                Color.White,
                0f,
                Vector2.Zero,
                0.65f,
                SpriteEffects.None,
                0f
            );
        }
    }
}