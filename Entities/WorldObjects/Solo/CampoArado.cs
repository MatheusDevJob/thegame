using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo;

public class CampoArado : Entity
{
    private enum EntityEstadoCampo
    {
        Arado,
        Molhado,
        Adubado
    }

    private EntityEstadoCampo EstadoCampo = EntityEstadoCampo.Arado;

    public override EntityRenderLayer RenderLayer => EntityRenderLayer.Ground;
    public CampoArado(GameContext context, Vector2 posicao) : base(context, "CampoArado", posicao)
    {
        BloqueiaMovimento = false;
        SpriteRow = 1;
        SpriteColumn = 1;
    }

    public void Molhar()
    {
        EstadoCampo = EntityEstadoCampo.Molhado;
        Sprite = EntityTexture2D.GetEntityTextureById(Context, "CampoAradoMolhado");
        SpriteRow = 0;
        SpriteColumn = 0;
    }

    public bool IsMolhado()
    {
        return EstadoCampo == EntityEstadoCampo.Molhado;
    }

}