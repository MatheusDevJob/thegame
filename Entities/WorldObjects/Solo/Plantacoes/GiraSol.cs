using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class GiraSol : Plantacao
{
    public GiraSol(GameContext context, Vector2 posicao) : base(context, "GiraSol", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 13;
        FrameHeight = 16;
        SegundosPorEstagio = 1;
        DropItemId = "GiraSol";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}