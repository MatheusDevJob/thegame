using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Rabanete : Plantacao
{
    public Rabanete(GameContext context, Vector2 posicao) : base(context, "Rabanete", posicao)
    {
        Preco = 3;
        BloqueiaMovimento = false;
        FrameWidth = 12;
        FrameHeight = 15;
        SegundosPorEstagio = 1;
        DropItemId = "Rabanete";
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio() { }
}