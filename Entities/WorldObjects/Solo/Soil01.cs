using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo;

public class Soil01 : Entity
{
    public Soil01(GameContext context, Vector2 posicao) : base(context, "Soil01", posicao)
    {
        FrameHeight = 12;
    }

    public override bool BloqueiaMovimento => false;
    public override EntityRenderLayer RenderLayer => EntityRenderLayer.Ground;
}