using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Trigo : Plantacao
{
    public Trigo(GameContext context, Vector2 posicao) : base(context, "Trigo", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 13;
        FrameHeight = 13;
        SegundosPorEstagio = 1;
        DropItemId = "Trigo";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}