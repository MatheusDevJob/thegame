using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.Tools;

public class AxeTool : Entity
{
    public AxeTool(GameContext context, Vector2 posicao) : base(context, "AxeTool", posicao, 0, 5)
    {
        FrameWidth = 15;
        FrameHeight = 13;
    }
}