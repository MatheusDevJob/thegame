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
        state.Inventory.RemoveItem(plantacao.Id, 1);

        Plantacao a = plantacao.Id switch
        {
            "SementeCenoura" => new Cenoura01(context, new Vector2(posicao.X * 16, posicao.Y * 16)),
            "SementeBeterraba" => new Beterraba01(context, new Vector2(posicao.X * 16, posicao.Y * 16)),
            _ => null,
        };
        if (a == null) return;
        a.SetSolo(solo);
        // a.DefinirPosicao(new Vector2(a.Posicao.X, a.Posicao.Y));

        state.EntityWorld.Add(a);
    }
}