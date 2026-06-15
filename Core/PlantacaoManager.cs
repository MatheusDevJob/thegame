using Microsoft.Xna.Framework;
using thegame.Entities;
using thegame.Entities.WorldObjects.Solo.Plantacoes;
using thegame.Maps;

namespace thegame.Core;

public static class PlantacaoManager
{
    public static void Plantar(GameContext context, Point posicao, Plantacao plantacao)
    {
        GameState state = context.State;
        int possuiItem = state.Inventory.PossuiItem(plantacao.Id);
        if (possuiItem < 1) return;

        state.EntityWorld.Add(EntityFactory.Create(context, new TiledObjectData
        {
            X = posicao.X,
            Y = posicao.Y,
            Type = plantacao.Id
        }));
    }
}