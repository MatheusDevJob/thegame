using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Items;

public class Tronco : Entity
{
    public Tronco(GameContext context, Vector2 posicao) : base(context, "Tronco", posicao, 3)
    {
        FrameWidth = 16;
        FrameHeight = 16;
        SpriteRow = 5;
        SpriteColumn = 31;
    }
}