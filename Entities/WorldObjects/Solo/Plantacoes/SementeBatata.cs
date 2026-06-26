using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class SementeBatata : Plantacao
{
    public SementeBatata(GameContext context, Vector2 posicao) : base(context, "SementeBatata", posicao)
    {
        Preco = 1;
        BloqueiaMovimento = false;
        FrameWidth = 7;
        FrameHeight = 7;
    }

    protected override void AtualizarSpritePorEstagio()
    {
        // throw new System.NotImplementedException();
    }

}