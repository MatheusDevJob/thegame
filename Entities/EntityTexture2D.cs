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
            "Wood" => context.Content.Load<Texture2D>("Items/wood"),
            "Pedra1" => context.Content.Load<Texture2D>("Cute_Fantasy_Free/Outdoor decoration/Outdoor_Decor_Free"),
            "Pedra2" => context.Content.Load<Texture2D>("Cute_Fantasy_Free/Outdoor decoration/Outdoor_Decor_Free"),
            "Bau" => context.Content.Load<Texture2D>("Items/spr_tileset_sunnysideworld_16px"),
            _ => throw new System.AccessViolationException($"Entidade não encontrada: {entityId}")
        };
    }
}