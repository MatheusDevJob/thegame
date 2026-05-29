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

    public float Life;
    protected Entity(GameContext context, Vector2 posicao, float life)
    {
        Context = context;
        Posicao = posicao;
        Life = life;
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