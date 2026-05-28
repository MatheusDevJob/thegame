using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thegame.Core;

public class Camera2D
{
    public Vector2 Position { get; private set; }
    public float Zoom { get; set; } = 2.2f;

    public void Follow(Vector2 target, Viewport viewport, int mapPixelWidth, int mapPixelHeight)
    {
        float halfViewWidth = viewport.Width / 2f / Zoom;
        float halfViewHeight = viewport.Height / 2f / Zoom;

        float minX = halfViewWidth;
        float maxX = mapPixelWidth - halfViewWidth;

        float minY = halfViewHeight;
        float maxY = mapPixelHeight - halfViewHeight;

        float cameraX;
        float cameraY;

        if (mapPixelWidth <= viewport.Width / Zoom)
            cameraX = mapPixelWidth / 2f;
        else
            cameraX = MathHelper.Clamp(target.X, minX, maxX);

        if (mapPixelHeight <= viewport.Height / Zoom)
            cameraY = mapPixelHeight / 2f;
        else
            cameraY = MathHelper.Clamp(target.Y, minY, maxY);

        Position = new Vector2(cameraX, cameraY);
    }

    public Matrix GetTransform(Viewport viewport)
    {
        return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f)) *
               Matrix.CreateScale(Zoom, Zoom, 1f) *
               Matrix.CreateTranslation(new Vector3(viewport.Width / 2f, viewport.Height / 2f, 0f));
    }
}