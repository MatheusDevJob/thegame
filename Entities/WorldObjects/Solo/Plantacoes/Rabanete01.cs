using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public class Rabanete01 : Plantacao
{
    public Rabanete01(GameContext context, Vector2 posicao) : base(context, "Rabanete01", posicao)
    {
        BloqueiaMovimento = false;
        FrameWidth = 6;
        FrameHeight = 6;
        SegundosPorEstagio = 1;
        DropItemId = "Rabanete";
        DropItemQtd = 1;
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        string NovoId;
        switch (EstagioAtual)
        {
            case 0:
                NovoId = "Rabanete01";
                FrameWidth = 6;
                FrameHeight = 6;

                break;

            case 1:
                NovoId = "Rabanete01";
                FrameWidth = 6;
                FrameHeight = 6;
                break;

            case 2:
                NovoId = "Rabanete02";
                FrameWidth = 8;
                FrameHeight = 7;
                break;

            case 3:
                NovoId = "Rabanete03";
                FrameWidth = 8;
                FrameHeight = 10;
                break;

            case 4:
                NovoId = "Rabanete04";
                FrameWidth = 12;
                FrameHeight = 15;
                break;

            default:
                NovoId = "Rabanete04";
                FrameWidth = 12;
                FrameHeight = 15;
                break;
        }

        AtualizarSprite(NovoId);
    }
}