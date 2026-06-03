using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Tools;

public class ShovelTool(GameContext context, Vector2 posicao) : Entity(context, "ShovelTool", posicao, 0, 1) { }