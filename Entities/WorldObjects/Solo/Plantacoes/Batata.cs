using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Batata : Plantacao
{
    public Batata(GameContext context, Vector2 posicao) : base(context, "Batata", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 12;
        FrameHeight = 12;
        SegundosPorEstagio = 1;
        DropItemId = "Batata";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}