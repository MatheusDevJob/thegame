using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class GiraSol01 : Plantacao
{
    public GiraSol01(GameContext context, Vector2 posicao) : base(context, "GiraSol01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 10;
        FrameHeight = 7;
        SegundosPorEstagio = 1;
        DropItemId = "GiraSol";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "GiraSol01";
                FrameWidth = 10;
                FrameHeight = 7;

                break;

            case 1:
                NovoId = "GiraSol01";
                FrameWidth = 10;
                FrameHeight = 7;
                break;

            case 2:
                NovoId = "GiraSol02";
                FrameWidth = 11;
                FrameHeight = 9;
                break;

            case 3:
                NovoId = "GiraSol03";
                FrameWidth = 11;
                FrameHeight = 13;
                break;

            case 4:
                NovoId = "GiraSol04";
                FrameWidth = 13;
                FrameHeight = 19;
                break;

            default:
                NovoId = "GiraSol04";
                FrameWidth = 13;
                FrameHeight = 19;
                break;
        }

        AtualizarSprite(NovoId);
    }
}