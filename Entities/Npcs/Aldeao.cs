using Microsoft.Xna.Framework;
using thegame.Core;
namespace thegame.Entities.Npcs;

public class Aldeao(GameContext Context, string nome, string fala, Vector2 posicao) : Npc(Context, nome, fala, posicao)
{
    protected override void NpcAction()
    {
        MostrarFala = !MostrarFala;
    }
}