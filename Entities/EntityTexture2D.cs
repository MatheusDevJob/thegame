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
            "AxeTool" => context.Content.Load<Texture2D>("Items/axe"),
            "Wood" => context.Content.Load<Texture2D>("Items/wood"),
            _ => null
        };
    }
}