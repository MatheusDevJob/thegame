using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Interactables;

public class Bau : Entity
{
    private const int TileSize = 16;
    public bool Aberto { get; private set; }

    public Bau(GameContext context, Vector2 posicao) : base(context, "Bau", posicao)
    {
        SpriteRow = 30;
        SpriteColumn = 38;
        BloqueiaMovimento = true;
        IsSpawnavel = true;
        AtualizarHitbox();
    }

    public override void Update(GameTime gameTime)
    {
        if (!Aberto) return;

        if (IsPlayerFartherThanMe(1))
            CloseBau();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (!Aberto) return;

        Rectangle sourceTampa = new(
            SpriteColumn * FrameWidth,
            (SpriteRow - 1) * FrameHeight,
            FrameWidth,
            FrameHeight
        );

        Rectangle destinationTampa = new(
            (int)(Posicao.X + DrawOffset.X),
            (int)(Posicao.Y + DrawOffset.Y - FrameHeight),
            FrameWidth,
            FrameHeight
        );

        spriteBatch.Draw(Sprite, destinationTampa, sourceTampa, Color.White);
    }


    public void OpenBau()
    {
        Aberto = true;
        SpriteColumn = 39;
    }

    public void CloseBau()
    {
        Aberto = false;
        SpriteColumn = 38;
    }
}