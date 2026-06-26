using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Chirivia : Plantacao
{
    public Chirivia(GameContext context, Vector2 posicao) : base(context, "Chirivia", posicao)
    {
        Preco = 37;
        BloqueiaMovimento = false;
        FrameWidth = 14;
        FrameHeight = 14;
        SegundosPorEstagio = 1;
        DropItemId = "Chirivia";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}