using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities.Items;
using thegame.Entities.Tools;
using thegame.Maps;

namespace thegame.Entities;

public static class EntityFactory
{
    public static Entity Create(GameContext context, TiledObjectData obj)
    {
        Vector2 posicao = new(obj.X, obj.Y);

        return obj.Type switch
        {
            "Pedra1" => new PedraPequena(context, posicao),
            "Pedra2" => new PedraMedia(context, posicao),

            "Tronco" => new Tronco(context, posicao),
            "AxeTool" => new AxeTool(context, posicao),
            "Wood" => new Wood(context, posicao),

            _ => null
        };
    }
}