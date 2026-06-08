using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Tools;

public class PickaxeTool : Entity
{
    public PickaxeTool(GameContext context, Vector2 posicao) : base(context, "PickaxeTool", posicao, 0, 1)
    {
        FrameWidth = 13;
        FrameHeight = 13;
    }
}