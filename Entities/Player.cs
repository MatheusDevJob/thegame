using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;

namespace thegame.Entities;

public class Player : Entity
{
    private readonly Texture2D _texture;
    public Vector2 Position
    {
        get => Posicao;
        set => DefinirPosicao(value);
    }

    public Vector2 Center => Posicao + new Vector2(_frameWidth * _scale / 2f, _frameHeight * _scale / 2f);

    private SpriteEffects _spriteEffects = SpriteEffects.None;
    private readonly int _frameWidth = 64;
    private readonly int _frameHeight = 34;
    private int _currentFrame;
    private int _currentRow;
    private double _timer;
    private readonly double _frameSpeed = 0.06;
    private readonly float _scale = 1f;
    private readonly float _speed = 60f;
    private readonly Texture2D _hitboxPixel;
    public bool _isAnimated;
    private Action _onAnimationFinished;
    private int _animationFrames;
    public bool IsAnimated => _isAnimated;
    public Player(GameContext context, GameSave save) : base(context, "Player", save.PlayerPosition, save.PlayerLife)
    {
        _texture = Sprite;
        _hitboxPixel = new Texture2D(Context.GraphicsDevice, 1, 1);
        _hitboxPixel.SetData([Color.White]);
        AtualizarHitbox();
    }

    public override void Update(GameTime gameTime)
    {
        Update(gameTime, null);
    }

    public void Update(GameTime gameTime, Func<Rectangle, bool> collides = null)
    {
        if (_isAnimated)
        {
            UpdateAnimation(gameTime, _animationFrames);
            return;
        }

        KeyboardState keyboard = Keyboard.GetState();

        Vector2 direction = Vector2.Zero;
        float speed = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboard.IsKeyDown(Keys.D))
        {
            direction.X += 1;
            _currentRow = 0;
            _spriteEffects = SpriteEffects.None;
        }

        if (keyboard.IsKeyDown(Keys.A))
        {
            direction.X -= 1;
            _currentRow = 0;
            _spriteEffects = SpriteEffects.FlipHorizontally;
        }

        if (keyboard.IsKeyDown(Keys.S))
        {
            direction.Y += 1;
            _currentRow = 0;
        }

        if (keyboard.IsKeyDown(Keys.W))
        {
            direction.Y -= 1;
            _currentRow = 0;
        }

        bool moving = direction != Vector2.Zero;
        Vector2 oldPosition = Posicao;

        if (moving)
        {
            direction.Normalize();
            Vector2 velocity = direction * speed;

            Vector2 nextPositionX = new(Posicao.X + velocity.X, Posicao.Y);

            if (collides == null || !collides(ObterHitboxFutura(nextPositionX)))
                DefinirPosicao(nextPositionX);

            Vector2 nextPositionY = new(Posicao.X, Posicao.Y + velocity.Y);

            if (collides == null || !collides(ObterHitboxFutura(nextPositionY)))
                DefinirPosicao(nextPositionY);

            if (Posicao != oldPosition)
                UpdateAnimation(gameTime, 8);
            else
                _currentFrame = 0;
        }
        else
        {
            _currentFrame = 0;
        }

        AtualizarHitbox();
    }

    public void PlayActionAnimation(int row, int frames, Action onFinished)
    {
        if (_isAnimated)
            return;

        _isAnimated = true;
        _currentFrame = 0;
        _currentRow = row;
        _timer = 0;
        _animationFrames = frames;
        _onAnimationFinished = onFinished;
    }

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        int width = (int)(_frameWidth * _scale);
        int height = (int)(_frameHeight * _scale);

        return new Rectangle(
            (int)posicao.X + (int)(width * 0.25f),
            (int)posicao.Y + (int)(height * 0.70f),
            (int)(14 * _scale),
            (int)(10 * _scale)
        );
    }

    private void UpdateAnimation(GameTime gameTime, int frames)
    {
        _timer += gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer < _frameSpeed)
            return;

        _timer = 0;
        _currentFrame++;

        if (_currentFrame < frames)
            return;

        _currentFrame = 0;
        _isAnimated = false;

        _onAnimationFinished?.Invoke();
        _onAnimationFinished = null;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRectangle = new(
            _currentFrame * _frameWidth,
            _currentRow * _frameHeight,
            _frameWidth,
            _frameHeight
        );

        Vector2 drawPosition = new(
            (float)Math.Round(Posicao.X),
            (float)Math.Round(Posicao.Y)
        );

        if (_spriteEffects == SpriteEffects.FlipHorizontally)
            drawPosition.X -= 19f * _scale;

        spriteBatch.Draw(
            _texture,
            drawPosition,
            sourceRectangle,
            Color.White,
            0f,
            Vector2.Zero,
            _scale,
            _spriteEffects,
            0f
        );

        // usado apenas para acompanhar a hitbox do player
        // DrawRectangle(spriteBatch, Hitbox, Color.Red, 1);
    }

    private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int thickness = 1)
    {
        spriteBatch.Draw(_hitboxPixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
        spriteBatch.Draw(_hitboxPixel, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
        spriteBatch.Draw(_hitboxPixel, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
        spriteBatch.Draw(_hitboxPixel, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
    }
}