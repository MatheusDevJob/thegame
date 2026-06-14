using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Tools;

public class WaterTool : Entity
{
    public WaterTool(GameContext context, Vector2 posicao) : base(context, "WaterTool", posicao)
    {
        FrameWidth = 13;
        FrameHeight = 13;
    }
}