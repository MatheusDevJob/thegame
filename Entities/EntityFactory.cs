using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities.Items;
using thegame.Entities.Items.WorldObjects.Interactables;
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

            // ferramentas
            "AxeTool" => new AxeTool(context, posicao),
            "PickaxeTool" => new PickaxeTool(context, posicao),
            "ShovelTool" => new ShovelTool(context, posicao),

            "Tronco" => new Tronco(context, posicao),

            // itens
            "Wood" => new Wood(context, posicao),
            "Dirt" => new Terra(context, posicao),

            // objetos do mundo
            "Bau" => new Bau(context, posicao),

            _ => null
        };
    }
}