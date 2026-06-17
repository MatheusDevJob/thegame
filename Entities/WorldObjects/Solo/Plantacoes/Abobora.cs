using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Abobora : Plantacao
{
    public Abobora(GameContext context, Vector2 posicao) : base(context, "Abobora", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 12;
        FrameHeight = 14;
        SegundosPorEstagio = 1;
        DropItemId = "Abobora";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}