using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Items;

public class PedraDrop(GameContext context, Vector2 posicao) : Entity(context, "PedraDrop", posicao)
{
    public override bool BloqueiaMovimento => false;
    public override bool IsColetavel => true;
}