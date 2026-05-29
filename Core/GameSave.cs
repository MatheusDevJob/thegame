using Microsoft.Xna.Framework;

namespace thegame.Core;

public class GameSave
{
    public float PlayerLife { get; set; } = 100f;
    public Vector2 PlayerPosition { get; set; } = new(1200, 200);
}