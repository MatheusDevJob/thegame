using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities;

public abstract class Monsters(GameContext context, Vector2 posicao, Player player) : Entity(context, "", posicao, 10)
{
    public int Health { get; protected set; }
    public float Speed { get; protected set; }

    protected Player Player = player;

    protected abstract void UpdateAI(GameTime gameTime);
    public override void Update(GameTime gameTime)
    {
        UpdateAI(gameTime);
    }
}
