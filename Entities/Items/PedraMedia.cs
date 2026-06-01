using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Items;

public class PedraMedia : Entity
{
    public PedraMedia(GameContext context, Vector2 posicao) : base(context, "Pedra2", posicao)
    {
        SpriteRow = 2;
        SpriteColumn = 2;
    }
}