using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class CouveFlor : Plantacao
{
    public CouveFlor(GameContext context, Vector2 posicao) : base(context, "CouveFlor", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 12;
        FrameHeight = 12;
        SegundosPorEstagio = 1;
        DropItemId = "CouveFlor";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}