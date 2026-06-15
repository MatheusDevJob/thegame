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
        AtualizarSpritePorEstagio();
    }


    protected override void AtualizarSpritePorEstagio()
    {
        switch (EstagioAtual)
        {
            case 0:
                Id = "Cenoura01";
                FrameWidth = 6;
                FrameHeight = 7;
                
                break;

            case 1:
                Id = "Cenoura01";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 2:
                Id = "Cenoura02";
                FrameWidth = 6;
                FrameHeight = 7;
                break;

            case 3:
                Id = "Cenoura03";
                FrameWidth = 8;
                FrameHeight = 10;
                break;

            case 4:
                Id = "Cenoura04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;

            default:
                Id = "Cenoura04";
                FrameWidth = 10;
                FrameHeight = 13;
                break;
        }

        AtualizarSprite(Id);
    }
}