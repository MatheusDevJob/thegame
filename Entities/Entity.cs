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
    public virtual bool IsColetavel => false;
    public float Life;
    public float Damage;
    public readonly Texture2D Sprite;
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

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);

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
        return new Rectangle((int)posicao.X, (int)posicao.Y, 32, 32);
    }
}