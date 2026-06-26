using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Batata : Plantacao
{
    public Batata(GameContext context, Vector2 posicao) : base(context, "Batata", posicao)
    {
        Preco = 7;
        BloqueiaMovimento = false;
        FrameWidth = 10;
        FrameHeight = 10;
        SegundosPorEstagio = 1;
        DropItemId = "Batata";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}