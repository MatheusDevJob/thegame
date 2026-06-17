using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Batata01 : Plantacao
{
    public Batata01(GameContext context, Vector2 posicao) : base(context, "Batata01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 7;
        FrameHeight = 7;
        SegundosPorEstagio = 1;
        DropItemId = "Batata";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Batata01";
                FrameWidth = 6;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "Batata01";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "Batata02";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 3:
                NovoId = "Batata03";
                FrameWidth = 8;
                FrameHeight = 10;
                break;

            case 4:
                NovoId = "Batata04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;

            default:
                NovoId = "Batata04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;
        }

        AtualizarSprite(NovoId);
    }
}