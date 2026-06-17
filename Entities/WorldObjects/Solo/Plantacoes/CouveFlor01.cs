using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class CouveFlor01 : Plantacao
{
    public CouveFlor01(GameContext context, Vector2 posicao) : base(context, "CouveFlor01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 5;
        FrameHeight = 5;
        SegundosPorEstagio = 1;
        DropItemId = "CouveFlor";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "CouveFlor01";
                FrameWidth = 6;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "CouveFlor01";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "CouveFlor02";
                FrameWidth = 8;
                FrameHeight = 8;
                break;

            case 3:
                NovoId = "CouveFlor03";
                FrameWidth = 10;
                FrameHeight = 11;
                break;

            case 4:
                NovoId = "CouveFlor04";
                FrameWidth = 12;
                FrameHeight = 12;
                break;

            default:
                NovoId = "CouveFlor04";
                FrameWidth = 12;
                FrameHeight = 12;
                break;
        }

        AtualizarSprite(NovoId);
    }
}