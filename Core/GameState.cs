

using thegame.Entities;

namespace thegame.Core;

public class GameState(GameContext context)
{
    public Player Player { get; } = new Player(context);
}