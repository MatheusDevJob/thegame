using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Repolho01 : Plantacao
{
    public Repolho01(GameContext context, Vector2 posicao) : base(context, "Repolho01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 5;
        FrameHeight = 5;
        SegundosPorEstagio = 1;
        DropItemId = "Repolho";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Repolho01";
                FrameWidth = 10;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "Repolho01";
                FrameWidth = 10;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "Repolho02";
                FrameWidth = 8;
                FrameHeight = 7;
                break;

            case 3:
                NovoId = "Repolho03";
                FrameWidth = 12;
                FrameHeight = 10;
                break;

            case 4:
                NovoId = "Repolho04";
                FrameWidth = 16;
                FrameHeight = 13;
                break;

            default:
                NovoId = "Repolho04";
                FrameWidth = 16;
                FrameHeight = 13;
                break;
        }

        AtualizarSprite(NovoId);
    }
}