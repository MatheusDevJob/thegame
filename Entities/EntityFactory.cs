using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities.Items;
using thegame.Entities.WorldObjects.Interactables;
using thegame.Entities.Tools;
using thegame.Maps;
using thegame.Entities.WorldObjects.Solo;
using thegame.Entities.WorldObjects.Solo.Plantacoes;

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
            "Soil" => new CampoArado(context, posicao),
            "Soil01" => new Soil01(context, posicao),
            "Soil02" => new Soil02(context, posicao),
            "Soil03" => new Soil03(context, posicao),

            // ferramentas
            "AxeTool" => new AxeTool(context, posicao),
            "PickaxeTool" => new PickaxeTool(context, posicao),
            "ShovelTool" => new ShovelTool(context, posicao),
            "WaterTool" => new WaterTool(context, posicao),

            "Tronco" => new Tronco(context, posicao),

            // itens
            "Wood" => new Wood(context, posicao),
            "Dirt" => new Terra(context, posicao),
            "PedraDrop" => new PedraDrop(context, posicao),

            // objetos do mundo
            "Bau" => new Bau(context, posicao),

            // FAZENDA
            "SementeCenoura" => new SementeCenoura(context, posicao),
            "Cenoura01" => new Cenoura01(context, posicao),
            "Cenoura" => new Cenoura(context, posicao),

            "SementeBeterraba" => new SementeBeterraba(context, posicao),
            "Beterraba01" => new Beterraba01(context, posicao),
            "Beterraba" => new Beterraba(context, posicao),

            "SementeRepolho" => new SementeRepolho(context, posicao),
            "Repolho01" => new Repolho01(context, posicao),
            "Repolho" => new Repolho(context, posicao),

            "SementeCouveFlor" => new SementeCouveFlor(context, posicao),
            "CouveFlor01" => new CouveFlor01(context, posicao),
            "CouveFlor" => new CouveFlor(context, posicao),

            "SementeCouve" => new SementeCouve(context, posicao),
            "Couve01" => new Couve01(context, posicao),
            "Couve" => new Couve(context, posicao),
  
            "SementeChirivia" => new SementeChirivia(context, posicao),
            "Chirivia01" => new Chirivia01(context, posicao),
            "Chirivia" => new Chirivia(context, posicao),
  
            "SementeBatata" => new SementeBatata(context, posicao),
            "Batata01" => new Batata01(context, posicao),
            "Batata" => new Batata(context, posicao),
            // "Cenoura03" => new Cenoura03(context, posicao),
            // "Cenoura04" => new Cenoura04(context, posicao),

            _ => null
        };
    }
}