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
    public CampoArado(GameContext context, Vector2 posicao) : base(context, "Soil", posicao)
    {
        BloqueiaMovimento = false;
        FrameHeight = 12;
    }

    public void Molhar()
    {
        if (Context.State.ActiveEquipe.Id != "WaterTool") return;
        EstadoCampo = EntityEstadoCampo.Molhado;
        Sprite = EntityTexture2D.GetEntityTextureById(Context, "Soil01");
    }

    public bool IsMolhado()
    {
        return EstadoCampo == EntityEstadoCampo.Molhado;
    }

}