using Microsoft.Xna.Framework;
using thegame.Core;
namespace thegame.Entities.Npcs;

public class Aldeao(GameContext Context, string nome, string fala, Vector2 posicao) : Npc(Context, nome, fala, posicao)
{
    public override void Update(GameTime gameTime)
    {
        UpdateAnimation(gameTime);
    }
}