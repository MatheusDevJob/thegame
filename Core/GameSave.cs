using System.Collections.Generic;
using Microsoft.Xna.Framework;
using thegame.Entities;

namespace thegame.Core;

public class GameSave
{
    public float PlayerLife { get; set; }
    public Vector2 PlayerPosition { get; set; }

    /* 
        itens do player
    */

    public List<string> ListTools { get; set; }
    public string ActiveTool { get; set; }

    public int BagLevel { get; set; }
}