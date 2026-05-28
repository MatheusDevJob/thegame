using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.Items;

public class Tronco : Entity
{
    private const int FrameWidth = 32;
    private const int FrameHeight = 48;

    private readonly Texture2D _sprite;

    public Tronco(GameContext context, Vector2 posicao) : base(context, posicao)
    {
        _sprite = context.Content.Load<Texture2D>("Cute_Fantasy_Free/Outdoor decoration/Oak_Tree_Small");
        AtualizarHitbox();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle destination = new(
            (int)Math.Round(Posicao.X),
            (int)Math.Round(Posicao.Y),
            FrameWidth,
            FrameHeight
        );

        Rectangle source = new(0, 0, FrameWidth, FrameHeight);

        spriteBatch.Draw(_sprite, destination, source, Color.White);
    }

    public override void Update(GameTime gameTime)
    {
    }

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        return new Rectangle(
            (int)posicao.X + 13,
            (int)posicao.Y + 30,
            6,
            5
        );
    }
}