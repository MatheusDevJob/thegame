using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Items;

public class PedraPequena : Entity
{
    public PedraPequena(GameContext context, Vector2 posicao) : base(context, "Pedra1", posicao, 3)
    {
        SpriteRow = 2;
        SpriteColumn = 1;
    }
}