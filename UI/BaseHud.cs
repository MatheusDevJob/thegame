using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities;
using thegame.Maps;

namespace thegame.UI;

public abstract class BaseHud
{
    protected readonly GameContext Context;
    protected readonly GameState gameState;
    protected readonly Texture2D nineSlice;
    protected readonly Texture2D _slotTexture;
    protected readonly Viewport viewport;

    protected readonly int screenWidth;
    protected readonly int screenHeight;
    protected readonly Texture2D _pixel;
    protected readonly SpriteFont fonte;

    protected BaseHud(GameContext context)
    {
        Context = context;
        gameState = context.State;

        nineSlice = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown_inlay");
        _slotTexture = context.Content.Load<Texture2D>("UI/Hud/itemdisc_01");

        viewport = context.GraphicsDevice.Viewport;

        screenWidth = viewport.Width;
        screenHeight = viewport.Height;

        fonte = context.Content.Load<SpriteFont>("Fonts/MenuFont");

        _pixel = new Texture2D(Context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(SpriteBatch spriteBatch);

    protected void DrawTextOutlined(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float scale)
    {
        spriteBatch.DrawString(fonte, text, position + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        spriteBatch.DrawString(fonte, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    protected static Rectangle GetBagSlotRectangle(Rectangle bagRect, int columns, int index)
    {
        int slotSize = 64;
        int gap = 2;

        int startX = bagRect.X + 40;
        int startY = bagRect.Y + 30;
        int row = index / columns;
        int col = index % columns;

        if (index > 7)
            startY += 20;

        return new Rectangle(
            startX + col * (slotSize + gap),
            startY + row * (slotSize + gap),
            slotSize,
            slotSize
        );
    }

    protected (Texture2D, Rectangle)? TryGetItemTexture(string itemId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return null;

            Entity entity = EntityFactory.Create(Context, new TiledObjectData
            {
                Type = itemId,
                X = 0,
                Y = 0
            });

            Rectangle source = new(
                entity.SpriteColumn * entity.FrameWidth,
                entity.SpriteRow * entity.FrameHeight,
                entity.FrameWidth,
                entity.FrameHeight
            );

            return (entity.Sprite, source);
        }
        catch
        {
            return null;
        }
    }

    protected static void DrawNineSlice(SpriteBatch spriteBatch, Texture2D texture, Rectangle dest, int sliceSize)
    {
        int w = texture.Width;
        int h = texture.Height;

        Rectangle srcTopLeft = new(0, 0, sliceSize, sliceSize);
        Rectangle srcTop = new(sliceSize, 0, w - sliceSize * 2, sliceSize);
        Rectangle srcTopRight = new(w - sliceSize, 0, sliceSize, sliceSize);
        Rectangle srcLeft = new(0, sliceSize, sliceSize, h - sliceSize * 2);
        Rectangle srcCenter = new(sliceSize, sliceSize, w - sliceSize * 2, h - sliceSize * 2);
        Rectangle srcRight = new(w - sliceSize, sliceSize, sliceSize, h - sliceSize * 2);
        Rectangle srcBottomLeft = new(0, h - sliceSize, sliceSize, sliceSize);
        Rectangle srcBottom = new(sliceSize, h - sliceSize, w - sliceSize * 2, sliceSize);
        Rectangle srcBottomRight = new(w - sliceSize, h - sliceSize, sliceSize, sliceSize);

        Rectangle dstTopLeft = new(dest.X, dest.Y, sliceSize, sliceSize);
        Rectangle dstTop = new(dest.X + sliceSize, dest.Y, dest.Width - sliceSize * 2, sliceSize);
        Rectangle dstTopRight = new(dest.Right - sliceSize, dest.Y, sliceSize, sliceSize);
        Rectangle dstLeft = new(dest.X, dest.Y + sliceSize, sliceSize, dest.Height - sliceSize * 2);
        Rectangle dstCenter = new(dest.X + sliceSize, dest.Y + sliceSize, dest.Width - sliceSize * 2, dest.Height - sliceSize * 2);
        Rectangle dstRight = new(dest.Right - sliceSize, dest.Y + sliceSize, sliceSize, dest.Height - sliceSize * 2);
        Rectangle dstBottomLeft = new(dest.X, dest.Bottom - sliceSize, sliceSize, sliceSize);
        Rectangle dstBottom = new(dest.X + sliceSize, dest.Bottom - sliceSize, dest.Width - sliceSize * 2, sliceSize);
        Rectangle dstBottomRight = new(dest.Right - sliceSize, dest.Bottom - sliceSize, sliceSize, sliceSize);

        spriteBatch.Draw(texture, dstTopLeft, srcTopLeft, Color.White);
        spriteBatch.Draw(texture, dstTop, srcTop, Color.White);
        spriteBatch.Draw(texture, dstTopRight, srcTopRight, Color.White);
        spriteBatch.Draw(texture, dstLeft, srcLeft, Color.White);
        spriteBatch.Draw(texture, dstCenter, srcCenter, Color.White);
        spriteBatch.Draw(texture, dstRight, srcRight, Color.White);
        spriteBatch.Draw(texture, dstBottomLeft, srcBottomLeft, Color.White);
        spriteBatch.Draw(texture, dstBottom, srcBottom, Color.White);
        spriteBatch.Draw(texture, dstBottomRight, srcBottomRight, Color.White);
    }
}