using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities;

public abstract class Entity
{
    protected readonly GameContext Context;

    public Vector2 Posicao { get; protected set; }
    public Rectangle Hitbox { get; protected set; }

    public virtual bool BloqueiaMovimento => true;
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
    protected int SpriteRow = 5;
    protected int SpriteColumn = 31;

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

    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Rectangle destination = new(
            (int)Math.Round(Posicao.X),
            (int)Math.Round(Posicao.Y),
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

    protected void DefinirPosicao(Vector2 novaPosicao)
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
}