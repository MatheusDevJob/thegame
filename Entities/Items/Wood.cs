using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.Items;

public class Wood(GameContext context, Vector2 position) : Entity(context, "Items/wood", "Wood", position)
{
    public override bool BloqueiaMovimento => false;
    public override bool IsColetavel => true;

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle destination = new(
            (int)Math.Round(Posicao.X),
            (int)Math.Round(Posicao.Y),
            11,
            11
        );
        spriteBatch.Draw(Sprite, destination, new Rectangle(0, 0, 11, 11), Color.White);
    }

    public override void Update(GameTime gameTime)
    {

    }

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        return new Rectangle(
            (int)posicao.X,
            (int)posicao.Y,
            11,
            11
        );
    }
}