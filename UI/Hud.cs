using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.UI;

public class Hud
{
    private readonly GameContext _context;
    private readonly Texture2D _pixel;
    private readonly Texture2D _hudBoxItem;
    private readonly Texture2D _hudAxeItem;
    private readonly int _scale = 3;

    public readonly List<int> boxes = [20, 74, 128, 182, 236, 290, 344];
    public int screenWidth;
    public int screenHeight;

    public Hud(GameContext context)
    {
        _context = context;
        _pixel = new Texture2D(_context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _hudBoxItem = context.Content.Load<Texture2D>("UI/Hud/itemdisc_01");
        _hudAxeItem = context.Content.Load<Texture2D>("UI/Hud/axe");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;

        screenWidth = viewport.Width;
        screenHeight = viewport.Height;

        Rectangle box = new(20, 20, 220, 70);

        spriteBatch.Draw(_pixel, box, new Color(0, 0, 0, 180));

        Rectangle lifeBackground = new(35, 45, 180, 14);
        Rectangle lifeBar = new(35, 45, (int)_context.State.Player.Life, 14);

        spriteBatch.Draw(_pixel, lifeBackground, new Color(80, 80, 80, 220));
        spriteBatch.Draw(_pixel, lifeBar, new Color(180, 40, 40, 255));



        Rectangle moneyBox = new(screenWidth - 220, 20, 200, 50);
        spriteBatch.Draw(_pixel, moneyBox, new Color(0, 0, 0));

        DrawItensBar(spriteBatch);
        DrawItensOnBar(spriteBatch);
    }

    private void DrawItensBar(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;

        int screenWidth = viewport.Width;
        int screenHeight = viewport.Height;

        int itemWidth = 18 * _scale;
        int itemHeight = 19 * _scale;

        int minX = boxes.Min();
        int maxX = boxes.Max() + itemWidth;
        int totalWidth = maxX - minX;

        int startX = (screenWidth - totalWidth) / 2;
        int y = screenHeight - 80;

        for (int i = 0; i < boxes.Count; i++)
        {
            int x = startX + (boxes[i] - minX);

            Rectangle bottomBox = new(x, y, itemWidth, itemHeight);
            spriteBatch.Draw(_hudBoxItem, bottomBox, Color.White);
        }
    }

    private void DrawItensOnBar(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;

        int screenWidth = viewport.Width;
        int screenHeight = viewport.Height;

        int itemWidth = 18 * _scale - 5;
        int itemHeight = 19 * _scale - 5;

        int minX = boxes.Min();
        int maxX = boxes.Max() + itemWidth;
        int totalWidth = maxX - minX;

        int startX = (screenWidth - totalWidth) / 2;
        int y = screenHeight - 80;

        for (int i = 0; i < boxes.Count; i++)
        {
            int x = startX + (boxes[i] - minX);

            Rectangle itemRect = new(
                x + 6,
                y + 4,
                itemWidth - 12,
                itemHeight - 8
            );

            spriteBatch.Draw(_hudAxeItem, itemRect, Color.White);
        }
    }
}