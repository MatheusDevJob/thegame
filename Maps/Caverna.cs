using System.Numerics;
using thegame.Core;

namespace thegame.Maps;

public class Caverna : BaseMap
{
    public Caverna(GameContext context) : base(context, "Caverna", "Maps/Caverna.tmj")
    {
        // context.State.Player.Posicao = new Vector2(100, 55);
    }
}