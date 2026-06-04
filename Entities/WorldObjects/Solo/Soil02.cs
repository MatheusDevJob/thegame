using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo;

public class Soil02 : Entity
{
    public Soil02(GameContext context, Vector2 posicao) : base(context, "Soil02", posicao)
    {
        FrameHeight = 12;
    }

    public override bool BloqueiaMovimento => false;
    public override EntityRenderLayer RenderLayer => EntityRenderLayer.Ground;
}