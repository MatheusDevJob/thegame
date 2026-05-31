using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace thegame.UI;

public class AnimatedBackground(Texture2D texture, int frameWidth, int frameHeight, int columns, int frameCount, double frameTime)
{
    private readonly Texture2D _texture = texture;
    private readonly int _frameWidth = frameWidth;
    private readonly int _frameHeight = frameHeight;
    private readonly int _columns = columns;
    private readonly int _frameCount = frameCount;
    private readonly double _frameTime = frameTime;
    private double _timer;
    private int _currentFrame;

    public void Update(GameTime gameTime)
    {
        _timer += gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer < _frameTime)
            return;

        _timer = 0;
        _currentFrame++;

        if (_currentFrame >= _frameCount)
            _currentFrame = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Viewport viewport)
    {
        int column = _currentFrame % _columns;
        int row = _currentFrame / _columns;

        Rectangle source = new(
            column * _frameWidth,
            row * _frameHeight,
            _frameWidth,
            _frameHeight
        );

        float scaleX = viewport.Width / (float)_frameWidth;
        float scaleY = viewport.Height / (float)_frameHeight;
        float scale = MathF.Max(scaleX, scaleY);

        int width = (int)(_frameWidth * scale);
        int height = (int)(_frameHeight * scale);

        Rectangle destination = new(
            (viewport.Width - width) / 2,
            (viewport.Height - height) / 2,
            width,
            height
        );

        spriteBatch.Draw(_texture, destination, source, Color.White);
    }
}