using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class SementeRabanete : Plantacao
{
    public SementeRabanete(GameContext context, Vector2 posicao) : base(context, "SementeRabanete", posicao)
    {
        Preco = 1;
        BloqueiaMovimento = false;
        FrameWidth = 6;
        FrameHeight = 6;
    }

    protected override void AtualizarSpritePorEstagio()
    {
        // throw new System.NotImplementedException();
    }

}