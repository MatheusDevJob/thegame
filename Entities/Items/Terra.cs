using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Items;

public class Terra(GameContext context, Vector2 posicao) : Entity(context, "Dirt", posicao)
{
    public override bool BloqueiaMovimento => false;
    public override bool IsColetavel => true;
}