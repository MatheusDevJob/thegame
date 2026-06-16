using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Beterraba01 : Plantacao
{
    public Beterraba01(GameContext context, Vector2 posicao) : base(context, "Beterraba01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 6;
        FrameHeight = 5;
        SegundosPorEstagio = 1;
        DropItemId = "Beterraba";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Beterraba01";
                FrameWidth = 6;
                FrameHeight = 5;

                break;

            case 1:
                NovoId = "Beterraba01";
                FrameWidth = 6;
                FrameHeight = 5;
                break;

            case 2:
                NovoId = "Beterraba02";
                FrameWidth = 8;
                FrameHeight = 6;
                break;

            case 3:
                NovoId = "Beterraba03";
                FrameWidth = 8;
                FrameHeight = 11;
                break;

            case 4:
                NovoId = "Beterraba04";
                FrameWidth = 12;
                FrameHeight = 14;
                break;

            default:
                NovoId = "Beterraba04";
                FrameWidth = 12;
                FrameHeight = 14;
                break;
        }

        AtualizarSprite(NovoId);
    }
}