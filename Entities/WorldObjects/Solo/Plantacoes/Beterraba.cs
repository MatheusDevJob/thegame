using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Beterraba : Plantacao
{
    public Beterraba(GameContext context, Vector2 posicao) : base(context, "Beterraba", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 16;
        FrameHeight = 16;
        IsColetavel = true;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    { }
}