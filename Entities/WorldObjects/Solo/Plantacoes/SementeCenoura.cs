using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class SementeCenoura : Plantacao
{
    public SementeCenoura(GameContext context, Vector2 posicao) : base(context, "SementeCenoura", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 7;
        FrameHeight = 7;
    }

    protected override void AtualizarSpritePorEstagio()
    {
        // throw new System.NotImplementedException();
    }

}