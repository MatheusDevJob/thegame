using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities.Items;
using thegame.Entities.WorldObjects.Interactables;
using thegame.Entities.Tools;
using thegame.Maps;
using thegame.Entities.WorldObjects.Solo;

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
            "Soil01" => new Soil01(context, posicao),
            "Soil02" => new Soil02(context, posicao),
            "Soil03" => new Soil03(context, posicao),

            // ferramentas
            "AxeTool" => new AxeTool(context, posicao),
            "PickaxeTool" => new PickaxeTool(context, posicao),
            "ShovelTool" => new ShovelTool(context, posicao),

            "Tronco" => new Tronco(context, posicao),

            // itens
            "Wood" => new Wood(context, posicao),
            "Dirt" => new Terra(context, posicao),
            "PedraDrop" => new PedraDrop(context, posicao),

            // objetos do mundo
            "Bau" => new Bau(context, posicao),

            _ => null
        };
    }
}