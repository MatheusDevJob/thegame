using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class SementeCouveFlor : Plantacao
{
    public SementeCouveFlor(GameContext context, Vector2 tilePosicao) : base(context, "SementeCouveFlor", tilePosicao)
    {
        Preco = 1;
        BloqueiaMovimento = false;
        FrameWidth = 5;
        FrameHeight = 5;
    }

    protected override void AtualizarSpritePorEstagio()
    {
        // throw new System.NotImplementedException();
    }
}