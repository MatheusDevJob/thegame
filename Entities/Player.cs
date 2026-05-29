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
    private readonly int _frameWidth = 32;
    private readonly int _frameHeight = 32;
    private int _currentFrame;
    private int _currentRow;
    private double _timer;
    private readonly double _frameSpeed = 0.12;
    private readonly float _scale = 1f;
    private readonly float _speed = 60f;

    public Player(GameContext context, GameSave save) : base(context, new Vector2(1200, 200), save.PlayerLife)
    {
        _texture = Context.Content.Load<Texture2D>("Cute_Fantasy_Free/Player/Player");
        AtualizarHitbox();
    }

    public override void Update(GameTime gameTime)
    {
        Update(gameTime, null);
    }

    public void Update(GameTime gameTime, Func<Rectangle, bool> collides = null)
    {
        KeyboardState keyboard = Keyboard.GetState();
        Vector2 direction = Vector2.Zero;
        float speed = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboard.IsKeyDown(Keys.D))
        {
            direction.X += 1;
            _currentRow = 1;
            _spriteEffects = SpriteEffects.None;
        }

        if (keyboard.IsKeyDown(Keys.A))
        {
            direction.X -= 1;
            _currentRow = 1;
            _spriteEffects = SpriteEffects.FlipHorizontally;
        }

        if (keyboard.IsKeyDown(Keys.S))
        {
            direction.Y += 1;
            _currentRow = 3;
        }

        if (keyboard.IsKeyDown(Keys.W))
        {
            direction.Y -= 1;
            _currentRow = 2;
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
                UpdateAnimation(gameTime);
            else
                _currentFrame = 0;
        }
        else
        {
            _currentFrame = 0;
        }

        AtualizarHitbox();
    }

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        int width = (int)(_frameWidth * _scale);
        int height = (int)(_frameHeight * _scale);

        return new Rectangle(
            (int)posicao.X + (int)(width * 0.25f),
            (int)posicao.Y + (int)(height * 0.65f),
            (int)(width * 0.5f),
            (int)(height * 0.25f)
        );
    }

    private void UpdateAnimation(GameTime gameTime)
    {
        _timer += gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer >= _frameSpeed)
        {
            _timer = 0;
            _currentFrame++;

            if (_currentFrame >= 6)
                _currentFrame = 0;
        }
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
    }

    public Rectangle GetHitbox()
    {
        return Hitbox;
    }
}