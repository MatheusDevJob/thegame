using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo;

// usaremos a Life dele como controle de "poder cavar mais", o limite é 3
public class Soil03 : Entity
{
    public Soil03(GameContext context, Vector2 posicao) : base(context, "Soil03", posicao)
    {
        FrameHeight = 12;
    }
    public override bool BloqueiaMovimento => false;
    public override EntityRenderLayer RenderLayer => EntityRenderLayer.Ground;
}