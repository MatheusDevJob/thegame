using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Couve : Plantacao
{
    public Couve(GameContext context, Vector2 posicao) : base(context, "Couve", posicao)
    {
        Preco = 7;
        BloqueiaMovimento = false;
        FrameWidth = 14;
        FrameHeight = 11;
        SegundosPorEstagio = 1;
        DropItemId = "Couve";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}