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
            "Cenoura1" => context.Content.Load<Texture2D>("Items/carrot_01"),
            "Cenoura2" => context.Content.Load<Texture2D>("Items/carrot_02"),
            "Cenoura3" => context.Content.Load<Texture2D>("Items/carrot_03"),
            "Cenoura4" => context.Content.Load<Texture2D>("Items/carrot_04"),
            "Cenoura" => context.Content.Load<Texture2D>("Items/carrot_05"),

            "CampoArado" => context.Content.Load<Texture2D>("Cute_Fantasy_Free/Tiles/Merged_image (1)"),
            "CampoAradoMolhado" => context.Content.Load<Texture2D>("Farming/CampoAradoMolhado"),
            _ => throw new System.AccessViolationException($"Entidade não encontrada: {entityId}")
        };
    }
}