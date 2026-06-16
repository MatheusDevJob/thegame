using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Repolho : Plantacao
{
    public Repolho(GameContext context, Vector2 posicao) : base(context, "Repolho", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 12;
        FrameHeight = 11;
        SegundosPorEstagio = 1;
        DropItemId = "Repolho";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}