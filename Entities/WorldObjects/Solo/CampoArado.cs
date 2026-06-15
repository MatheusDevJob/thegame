using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo;

public class CampoArado : Entity
{
    private const int TileSize = 16;
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
    public override void Draw(SpriteBatch spriteBatch)
    {
        int offsetY = (TileSize - FrameHeight) / 2;

        Rectangle destination = new(
            (int)(Posicao.X + DrawOffset.X),
            (int)(Posicao.Y + offsetY + DrawOffset.Y),
            FrameWidth,
            FrameHeight
        );

        Rectangle source = new(
            SpriteColumn * FrameWidth,
            SpriteRow * FrameHeight,
            FrameWidth,
            FrameHeight
        );

        spriteBatch.Draw(Sprite, destination, source, Color.White);
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