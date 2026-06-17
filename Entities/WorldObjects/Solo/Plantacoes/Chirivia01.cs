using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Chirivia01 : Plantacao
{
    public Chirivia01(GameContext context, Vector2 posicao) : base(context, "Chirivia01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 6;
        FrameHeight = 6;
        SegundosPorEstagio = 1;
        DropItemId = "Chirivia";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Chirivia01";
                FrameWidth = 6;
                FrameHeight = 6;

                break;

            case 1:
                NovoId = "Chirivia01";
                FrameWidth = 6;
                FrameHeight = 6;
                break;

            case 2:
                NovoId = "Chirivia02";
                FrameWidth = 8;
                FrameHeight = 7;
                break;

            case 3:
                NovoId = "Chirivia03";
                FrameWidth = 8;
                FrameHeight = 8;
                break;

            case 4:
                NovoId = "Chirivia04";
                FrameWidth = 10;
                FrameHeight = 14;
                break;

            default:
                NovoId = "Chirivia04";
                FrameWidth = 10;
                FrameHeight = 14;
                break;
        }

        AtualizarSprite(NovoId);
    }
}