using Microsoft.Xna.Framework;
using thegame.Entities.WorldObjects.Solo;

namespace thegame.Core;

public static class Farming
{

    public static void ArarCampo(GameContext context, Point tile)
    {
        int escala = 16;
        Rectangle area = new(
            tile.X + escala,
            tile.Y + escala,
            escala,
            escala
        );

        context.State.EntityWorld.Add(new CampoArado(context, new Vector2(tile.X * 16, tile.Y * 16)));
    }
}