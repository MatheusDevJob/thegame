using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Tools;

public class PickaxeTool(GameContext context, Vector2 posicao) : Entity(context, "PickaxeTool", posicao, 0, 1) { }