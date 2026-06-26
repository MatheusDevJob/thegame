using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Cenoura : Plantacao
{
    public Cenoura(GameContext context, Vector2 posicao) : base(context, "Cenoura", posicao)
    {
        Preco = 12;
        BloqueiaMovimento = false;
        FrameWidth = 12;
        FrameHeight = 12;
        SegundosPorEstagio = 1;
        DropItemId = "Cenoura";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}