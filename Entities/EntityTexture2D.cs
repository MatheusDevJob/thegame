using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities;

public static class EntityTexture2D
{
    public static Texture2D GetEntityTextureById(GameContext context, string entityId)
    {
        return entityId switch
        {
            "Aldeão" => context.Content.Load<Texture2D>("npc-pack/Old man/Old man walk-Sheet"),
            "Player" => context.Content.Load<Texture2D>("Player/spr_basecharacter_allframes"),
            "Tronco" => context.Content.Load<Texture2D>("Items/spr_tileset_sunnysideworld_16px"),
            // Ferramentas
            "ShovelTool" => context.Content.Load<Texture2D>("Items/shovel"),
            "AxeTool" => context.Content.Load<Texture2D>("Items/axe"),
            "PickaxeTool" => context.Content.Load<Texture2D>("Items/pickaxe"),
            "WaterTool" => context.Content.Load<Texture2D>("Items/Tools/water"),

            // Itens
            "Dirt" => context.Content.Load<Texture2D>("Items/dirt"),
            "Wood" => context.Content.Load<Texture2D>("Items/wood"),
            "PedraDrop" => context.Content.Load<Texture2D>("Items/stone"),

            "Pedra1" => context.Content.Load<Texture2D>("Cute_Fantasy_Free/Outdoor decoration/Outdoor_Decor_Free"),
            "Pedra2" => context.Content.Load<Texture2D>("Cute_Fantasy_Free/Outdoor decoration/Outdoor_Decor_Free"),
            "Soil" => context.Content.Load<Texture2D>("Items/Soil_00"),
            "Soil01" => context.Content.Load<Texture2D>("Items/Soil_01"),
            "Soil02" => context.Content.Load<Texture2D>("Items/Soil_03"),
            "Soil03" => context.Content.Load<Texture2D>("Items/Soil_04"),
            "Bau" => context.Content.Load<Texture2D>("Items/spr_tileset_sunnysideworld_16px"),

            // PLANTAÇÃO
            "SementeCenoura" => context.Content.Load<Texture2D>("Items/carrot_00"),
            "Cenoura01" => context.Content.Load<Texture2D>("Items/carrot_01"),
            "Cenoura02" => context.Content.Load<Texture2D>("Items/carrot_02"),
            "Cenoura03" => context.Content.Load<Texture2D>("Items/carrot_03"),
            "Cenoura04" => context.Content.Load<Texture2D>("Items/carrot_04"),
            "Cenoura" => context.Content.Load<Texture2D>("Items/carrot_05"),

            "SementeBeterraba" => context.Content.Load<Texture2D>("Items/beetroot_00"),
            "Beterraba01" => context.Content.Load<Texture2D>("Items/beetroot_01"),
            "Beterraba02" => context.Content.Load<Texture2D>("Items/beetroot_02"),
            "Beterraba03" => context.Content.Load<Texture2D>("Items/beetroot_03"),
            "Beterraba04" => context.Content.Load<Texture2D>("Items/beetroot_04"),
            "Beterraba" => context.Content.Load<Texture2D>("Items/beetroot_05"),

            "SementeRepolho" => context.Content.Load<Texture2D>("Items/cabbage_00"),
            "Repolho01" => context.Content.Load<Texture2D>("Items/cabbage_01"),
            "Repolho02" => context.Content.Load<Texture2D>("Items/cabbage_02"),
            "Repolho03" => context.Content.Load<Texture2D>("Items/cabbage_03"),
            "Repolho04" => context.Content.Load<Texture2D>("Items/cabbage_04"),
            "Repolho" => context.Content.Load<Texture2D>("Items/cabbage_05"),

            "SementeCouveFlor" => context.Content.Load<Texture2D>("Items/cauliflower_00"),
            "CouveFlor01" => context.Content.Load<Texture2D>("Items/cauliflower_01"),
            "CouveFlor02" => context.Content.Load<Texture2D>("Items/cauliflower_02"),
            "CouveFlor03" => context.Content.Load<Texture2D>("Items/cauliflower_03"),
            "CouveFlor04" => context.Content.Load<Texture2D>("Items/cauliflower_04"),
            "CouveFlor" => context.Content.Load<Texture2D>("Items/cauliflower_05"),

            "SementeCouve" => context.Content.Load<Texture2D>("Items/kale_00"),
            "Couve01" => context.Content.Load<Texture2D>("Items/kale_01"),
            "Couve02" => context.Content.Load<Texture2D>("Items/kale_02"),
            "Couve03" => context.Content.Load<Texture2D>("Items/kale_03"),
            "Couve04" => context.Content.Load<Texture2D>("Items/kale_04"),
            "Couve" => context.Content.Load<Texture2D>("Items/kale_05"),

            "SementeChirivia" => context.Content.Load<Texture2D>("Items/pastinaca_00"),
            "Chirivia01" => context.Content.Load<Texture2D>("Items/pastinaca_01"),
            "Chirivia02" => context.Content.Load<Texture2D>("Items/pastinaca_02"),
            "Chirivia03" => context.Content.Load<Texture2D>("Items/pastinaca_03"),
            "Chirivia04" => context.Content.Load<Texture2D>("Items/pastinaca_04"),
            "Chirivia" => context.Content.Load<Texture2D>("Items/pastinaca_05"),

            "SementeBatata" => context.Content.Load<Texture2D>("Items/potato_00"),
            "Batata01" => context.Content.Load<Texture2D>("Items/potato_01"),
            "Batata02" => context.Content.Load<Texture2D>("Items/potato_02"),
            "Batata03" => context.Content.Load<Texture2D>("Items/potato_03"),
            "Batata04" => context.Content.Load<Texture2D>("Items/potato_04"),
            "Batata" => context.Content.Load<Texture2D>("Items/potato_05"),

            "SementeAbobora" => context.Content.Load<Texture2D>("Items/pumpkin_00"),
            "Abobora01" => context.Content.Load<Texture2D>("Items/pumpkin_01"),
            "Abobora02" => context.Content.Load<Texture2D>("Items/pumpkin_02"),
            "Abobora03" => context.Content.Load<Texture2D>("Items/pumpkin_03"),
            "Abobora04" => context.Content.Load<Texture2D>("Items/pumpkin_04"),
            "Abobora" => context.Content.Load<Texture2D>("Items/pumpkin_05"),

            "CampoArado" => context.Content.Load<Texture2D>("Cute_Fantasy_Free/Tiles/Merged_image (1)"),
            "CampoAradoMolhado" => context.Content.Load<Texture2D>("Farming/CampoAradoMolhado"),
            _ => throw new System.AccessViolationException($"Entidade não encontrada: {entityId}")
        };
    }
}