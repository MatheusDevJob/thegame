using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Items;

public class Terra : Entity
{
    public override bool BloqueiaMovimento => false;
    public override bool IsColetavel => true;
    public Terra(GameContext context, Vector2 posicao) : base(context, "Dirt", posicao)
    {
        SpriteRow = 0;
        SpriteColumn = 0;
    }
}