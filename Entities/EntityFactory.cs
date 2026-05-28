using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities.Items;
using thegame.Entities.Npcs;
using thegame.Maps;

namespace thegame.Entities;

public static class EntityFactory
{
    public static Entity Create(GameContext context, TiledObjectData obj)
    {
        Vector2 posicao = new(obj.X, obj.Y);

        return obj.Type switch
        {
            // "Pedra" => new Pedra(context, posicao),

            "tronco" => new Tronco(context, posicao),

            _ => null
        };
    }
}