using System;
using System.Collections.Generic;
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

        TilePosition = new Point(
            (int)Math.Floor(WorldPosition.X / _tileSize),
            (int)Math.Floor(WorldPosition.Y / _tileSize)
        );
    }

    public Rectangle GetTileRectangle()
    {
        return new Rectangle(TilePosition.X * _tileSize, TilePosition.Y * _tileSize, _tileSize, _tileSize);
    }

    public Dictionary<string, Point> GetCardinalTiles()
    {
        Point tile = TilePosition;

        return new Dictionary<string, Point>
        {
            ["Center"] = tile,
            ["Up"] = new Point(tile.X, tile.Y - 1),
            ["Right"] = new Point(tile.X + 1, tile.Y),
            ["Down"] = new Point(tile.X, tile.Y + 1),
            ["Left"] = new Point(tile.X - 1, tile.Y)
        };
    }

    public Dictionary<string, Point> GetSurroundingTiles()
    {
        Point tile = TilePosition;

        return new Dictionary<string, Point>
        {
            ["Center"] = tile,
            ["Up"] = new Point(tile.X, tile.Y - 1),
            ["UpRight"] = new Point(tile.X + 1, tile.Y - 1),
            ["Right"] = new Point(tile.X + 1, tile.Y),
            ["DownRight"] = new Point(tile.X + 1, tile.Y + 1),
            ["Down"] = new Point(tile.X, tile.Y + 1),
            ["DownLeft"] = new Point(tile.X - 1, tile.Y + 1),
            ["Left"] = new Point(tile.X - 1, tile.Y),
            ["UpLeft"] = new Point(tile.X - 1, tile.Y - 1)
        };
    }

    public List<Point> GetTilesInRadius(int radius = 1, bool includeCenter = true)
    {
        List<Point> tiles = [];
        Point center = TilePosition;

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (!includeCenter && x == 0 && y == 0)
                    continue;

                tiles.Add(new Point(center.X + x, center.Y + y));
            }
        }

        return tiles;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_pixel, GetTileRectangle(), Color.Yellow * 0.35f);
    }
}