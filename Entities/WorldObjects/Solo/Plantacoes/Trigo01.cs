using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Trigo01 : Plantacao
{
    public Trigo01(GameContext context, Vector2 posicao) : base(context, "Trigo01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 7;
        FrameHeight = 7;
        SegundosPorEstagio = 1;
        DropItemId = "Trigo";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Trigo01";
                FrameWidth = 11;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "Trigo01";
                FrameWidth = 11;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "Trigo02";
                FrameWidth = 11;
                FrameHeight = 10;
                break;

            case 3:
                NovoId = "Trigo03";
                FrameWidth = 13;
                FrameHeight = 14;
                break;

            case 4:
                NovoId = "Trigo04";
                FrameWidth = 13;
                FrameHeight = 16;
                break;

            default:
                NovoId = "Trigo04";
                FrameWidth = 13;
                FrameHeight = 16;
                break;
        }

        AtualizarSprite(NovoId);
    }
}