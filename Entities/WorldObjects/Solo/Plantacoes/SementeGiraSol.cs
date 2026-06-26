using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class SementeGiraSol : Plantacao
{
    public SementeGiraSol(GameContext context, Vector2 posicao) : base(context, "SementeGiraSol", posicao)
    {
        Preco = 1;
        BloqueiaMovimento = false;
        FrameWidth = 7;
        FrameHeight = 7;
    }

    protected override void AtualizarSpritePorEstagio()
    {
        // throw new System.NotImplementedException();
    }

}