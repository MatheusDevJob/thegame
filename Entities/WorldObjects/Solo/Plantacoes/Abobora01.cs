using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Abobora01 : Plantacao
{
    public Abobora01(GameContext context, Vector2 posicao) : base(context, "Abobora01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 7;
        FrameHeight = 7;
        SegundosPorEstagio = 1;
        DropItemId = "Abobora";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Abobora01";
                FrameWidth = 6;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "Abobora01";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "Abobora02";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 3:
                NovoId = "Abobora03";
                FrameWidth = 8;
                FrameHeight = 10;
                break;

            case 4:
                NovoId = "Abobora04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;

            default:
                NovoId = "Abobora04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;
        }

        AtualizarSprite(NovoId);
    }
}