

using thegame.Entities;

namespace thegame.Core;

public class GameState(GameContext context, GameSave gameSave)
{
    public GameSave PlayerSave = gameSave;
    public Player Player { get; } = new Player(context, gameSave);
    public bool LayoutMenu = false;
}