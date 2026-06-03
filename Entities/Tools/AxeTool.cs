using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.Tools;

public class AxeTool(GameContext context, Vector2 posicao) : Entity(context, "AxeTool", posicao, 0, 5) { }