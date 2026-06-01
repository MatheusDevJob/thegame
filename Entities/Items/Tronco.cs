using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.Items;

public class Tronco : Entity
{
    private const int FrameWidth = 16;
    private const int FrameHeight = 16;
    private const int SpriteRow = 5;
    private const int SpriteColumn = 31;

    private readonly Texture2D _sprite;

    public Tronco(GameContext context, Vector2 posicao) : base(context, "Tronco", posicao, 3)
    {
        _sprite = Sprite;
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

        Rectangle source = new(
            SpriteColumn * FrameWidth,
            SpriteRow * FrameHeight,
            FrameWidth,
            FrameHeight
        );

        spriteBatch.Draw(_sprite, destination, source, Color.White);
    }

    public override void Update(GameTime gameTime)
    {
    }

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        return new Rectangle(
            (int)posicao.X,
            (int)posicao.Y,
            FrameWidth,
            FrameHeight
        );
    }
}