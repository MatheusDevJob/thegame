using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Cenoura01 : Plantacao
{
    public Cenoura01(GameContext context, Vector2 posicao) : base(context, "Cenoura01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 7;
        FrameHeight = 7;
        SegundosPorEstagio = 1;
        DropItemId = "Cenoura";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Cenoura01";
                FrameWidth = 6;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "Cenoura01";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "Cenoura02";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 3:
                NovoId = "Cenoura03";
                FrameWidth = 8;
                FrameHeight = 10;
                break;

            case 4:
                NovoId = "Cenoura04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;

            default:
                NovoId = "Cenoura04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;
        }

        AtualizarSprite(NovoId);
    }
}