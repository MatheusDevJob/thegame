using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace thegame.Core;

public class TileCursor
{
    private readonly Texture2D _pixel;
    private readonly int _tileSize;
    public Point TilePosition { get; private set; }
    public Vector2 WorldPosition { get; private set; }
    private readonly GameContext _context;

    public TileCursor(GameContext context, int tileSize = 16)
    {
        _tileSize = tileSize;
        _context = context;
        _pixel = new Texture2D(context.GraphicsDevice, 1, 1);

        _pixel.SetData([Color.White]);
    }

    public void Update(Camera2D camera)
    {
        MouseState mouse = Mouse.GetState();
        Vector2 mouseScreen = new(mouse.X, mouse.Y);
        Matrix inverseCamera = Matrix.Invert(camera.GetTransform(_context.GraphicsDevice.Viewport));
        WorldPosition = Vector2.Transform(mouseScreen, inverseCamera);
        TilePosition = new Point((int)(WorldPosition.X / _tileSize), (int)(WorldPosition.Y / _tileSize));
    }

    public Rectangle GetTileRectangle()
    {
        return new Rectangle(TilePosition.X * _tileSize, TilePosition.Y * _tileSize, _tileSize, _tileSize);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_pixel, GetTileRectangle(), Color.Yellow * 0.35f);
    }
}