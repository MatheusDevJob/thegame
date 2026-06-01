using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Items;

public class Wood : Entity
{
    public Wood(GameContext context, Vector2 posicao) : base(context, "Wood", posicao)
    {
        FrameWidth = 11;
        FrameHeight = 11;
        SpriteRow = 0;
        SpriteColumn = 0;
    }

    public override bool BloqueiaMovimento => false;
    public override bool IsColetavel => true;
}