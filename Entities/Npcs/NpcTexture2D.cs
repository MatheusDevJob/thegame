using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.Npcs;

public static class NpcTexture2D
{
    public static Texture2D GetNpcPortraitById(GameContext context, string TextureId)
    {
        return TextureId switch
        {
            "Aldeão" => context.Content.Load<Texture2D>("npc-pack/Old man/Old man portrait"),
            "Eduarda" => context.Content.Load<Texture2D>("npc-pack/Woman/Woman portrait"),
            "Soldado" => context.Content.Load<Texture2D>("npc-pack/Soldier/Soldier portrait"),
            "CaminhaoVenda" => context.Content.Load<Texture2D>("npc-pack/Soldier/Soldier portrait"),

            _ => throw new System.AccessViolationException($"Npc não encontrado: {TextureId}")
        };
    }
}