using Microsoft.Xna.Framework;
using thegame.Entities;
using thegame.Entities.WorldObjects.Solo;
using thegame.Entities.WorldObjects.Solo.Plantacoes;

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

    public static void Plantar(GameContext context, Point posicao, Plantacao plantacao, Entity solo)
    {
        GameState state = context.State;
        int possuiItem = state.Inventory.PossuiItem(plantacao.Id);
        if (possuiItem < 1) return;

        Plantacao a = plantacao.Id switch
        {
            "SementeCenoura" => new Cenoura01(context, new Vector2(posicao.X, posicao.Y)),
            _ => null,
        };
        a.SetSolo(solo);
        // a.DefinirPosicao(new Vector2(a.Posicao.X, a.Posicao.Y));

        state.EntityWorld.Add(a);
    }
}