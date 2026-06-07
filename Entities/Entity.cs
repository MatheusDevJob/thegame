using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities;

public enum EntityRenderLayer
{
    Ground,
    Normal,
    Top
}

public abstract class Entity
{
    public virtual EntityRenderLayer RenderLayer => EntityRenderLayer.Normal;
    protected readonly GameContext Context;

    public Vector2 Posicao { get; protected set; }
    public Rectangle Hitbox { get; protected set; }

    public virtual bool BloqueiaMovimento { get; set; } = true;
    public virtual float SortY => Hitbox.Bottom;

    public string Id;
    public string SaveId { get; set; } = "";
    public bool Persistente { get; set; } = true;
    public virtual bool IsColetavel => false;
    public float Life;
    public float Damage;
    public readonly Texture2D Sprite;
    protected int FrameWidth = 16;
    protected int FrameHeight = 16;
    protected int SpriteRow = 0;
    protected int SpriteColumn = 0;

    protected Entity(GameContext context, string id, Vector2 posicao, float life = 0, float damage = 0)
    {
        Context = context;
        Posicao = posicao;
        Id = id;
        Life = life;
        Damage = damage;
        Sprite = EntityTexture2D.GetEntityTextureById(context, id);
        AtualizarHitbox();
    }

    public virtual void Update(GameTime gameTime)
    {
        UpdateShake(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Rectangle destination = new(
            (int)(Posicao.X + DrawOffset.X),
            (int)(Posicao.Y + DrawOffset.Y),
            FrameWidth,
            FrameHeight
        );

        Rectangle source = new(
            SpriteColumn * FrameWidth,
            SpriteRow * FrameHeight,
            FrameWidth,
            FrameHeight
        );

        spriteBatch.Draw(Sprite, destination, source, Color.White);
    }

    public Rectangle ObterHitboxFutura(Vector2 novaPosicao)
    {
        return CalcularHitbox(novaPosicao);
    }

    public void DefinirPosicao(Vector2 novaPosicao)
    {
        Posicao = novaPosicao;
        AtualizarHitbox();
    }

    protected virtual void AtualizarHitbox()
    {
        Hitbox = CalcularHitbox(Posicao);
    }

    protected virtual Rectangle CalcularHitbox(Vector2 posicao)
    {
        return new Rectangle(
            (int)posicao.X,
            (int)posicao.Y,
            FrameWidth,
            FrameHeight
        );
    }

    protected bool IsPlayerFartherThanMe(int maxTiles = 2)
    {
        const int tileSize = 16;

        if (Context.State.Player == null) return true;

        Point playerTile = new(
            Context.State.Player.Hitbox.Center.X / tileSize,
            Context.State.Player.Hitbox.Center.Y / tileSize
        );

        Point entityTile = new(
            Hitbox.Center.X / tileSize,
            Hitbox.Center.Y / tileSize
        );

        int distanceX = Math.Abs(entityTile.X - playerTile.X);
        int distanceY = Math.Abs(entityTile.Y - playerTile.Y);

        return distanceX > maxTiles || distanceY > maxTiles;
    }

    private float _shakeTimer;
    private float _shakeDuration;
    private float _shakeStrength;
    private static readonly Random _random = new();

    public Vector2 DrawOffset { get; private set; } = Vector2.Zero;

    public void Shake(float duration = 0.15f, float strength = 2f)
    {
        _shakeDuration = duration;
        _shakeTimer = duration;
        _shakeStrength = strength;
    }

    protected void UpdateShake(GameTime gameTime)
    {
        if (_shakeTimer <= 0)
        {
            DrawOffset = Vector2.Zero;
            return;
        }

        _shakeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        float progress = _shakeTimer / _shakeDuration;
        float force = _shakeStrength * progress;

        DrawOffset = new Vector2(
            (float)(_random.NextDouble() * 2 - 1) * force,
            (float)(_random.NextDouble() * 2 - 1) * force
        );

        if (_shakeTimer <= 0)
            DrawOffset = Vector2.Zero;
    }
}