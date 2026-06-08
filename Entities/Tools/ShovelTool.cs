using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Tools;

public class ShovelTool : Entity
{
    public ShovelTool(GameContext context, Vector2 posicao) : base(context, "ShovelTool", posicao, 0, 1)
    {
        FrameWidth = 14;
        FrameHeight = 14;
    }
}