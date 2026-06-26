using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class SementeRepolho : Plantacao
{
    public SementeRepolho(GameContext context, Vector2 tilePosicao) : base(context, "SementeRepolho", tilePosicao)
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