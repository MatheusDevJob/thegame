using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class SementeBeterraba : Plantacao
{
    public SementeBeterraba(GameContext context, Vector2 tilePosicao) : base(context, "SementeBeterraba", tilePosicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 5;
        FrameHeight = 5;
    }

    protected override void AtualizarSpritePorEstagio()
    {
        // throw new System.NotImplementedException();
    }
}