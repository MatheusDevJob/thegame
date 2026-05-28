using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.UI;

public class Hud
{
    private readonly GameContext _context;
    private readonly Texture2D _pixel;

    public Hud(GameContext context)
    {
        _context = context;
        _pixel = new Texture2D(_context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;

        int screenWidth = viewport.Width;
        int screenHeight = viewport.Height;

        Rectangle box = new(20, 20, 220, 70);

        spriteBatch.Draw(_pixel, box, new Color(0, 0, 0, 180));

        Rectangle lifeBackground = new(35, 45, 180, 14);
        Rectangle lifeBar = new(35, 45, 140, 14);

        spriteBatch.Draw(_pixel, lifeBackground, new Color(80, 80, 80, 220));
        spriteBatch.Draw(_pixel, lifeBar, new Color(180, 40, 40, 255));



        Rectangle moneyBox = new(screenWidth - 220, 20, 200, 50);
        spriteBatch.Draw(_pixel, moneyBox, new Color(0, 0, 0));


        Rectangle bottomBox = new(20, screenHeight - 80, 300, 60);
        spriteBatch.Draw(_pixel, bottomBox, new Color(0, 0, 0));
    }
}