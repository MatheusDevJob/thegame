using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Couve01 : Plantacao
{
    public Couve01(GameContext context, Vector2 posicao) : base(context, "Couve01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 5;
        FrameHeight = 5;
        SegundosPorEstagio = 1;
        DropItemId = "Couve";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Couve01";
                FrameWidth = 4;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "Couve01";
                FrameWidth = 4;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "Couve02";
                FrameWidth = 6;
                FrameHeight = 8;
                break;

            case 3:
                NovoId = "Couve03";
                FrameWidth = 10;
                FrameHeight = 9;
                break;

            case 4:
                NovoId = "Couve04";
                FrameWidth = 14;
                FrameHeight = 11;
                break;

            default:
                NovoId = "Couve04";
                FrameWidth = 14;
                FrameHeight = 11;
                break;
        }

        AtualizarSprite(NovoId);
    }
}