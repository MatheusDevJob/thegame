using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public abstract class Plantacao : Vendivel
{
    private const int TileSize = 16;

    protected int EstagioAtual = 0;
    protected int EstagioMaximo = 4;
    protected int SegundosPorEstagio = 30;

    public bool IsColhivel { get; protected set; } = false;
    public string DropItemId { get; protected set; }
    public int DropItemQtd { get; protected set; }

    protected double PlantadoEmTempoJogo;

    private double _timerVerificacao;
    public Dictionary<string, string> InfoPlantacao = [];
    private Entity Solo;

    protected Plantacao(GameContext context, string entityId, Vector2 tilePosicao)
        : base(context, entityId, tilePosicao)
    {
        BloqueiaMovimento = false;
        IsColetavel = false;

        PlantadoEmTempoJogo = Context.State.TempoJogoSegundos;

        InfoPlantacao["plantadoEmTempoJogo"] = PlantadoEmTempoJogo.ToString();
        InfoPlantacao["estagio"] = EstagioAtual.ToString();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        float tileCenterX = Posicao.X + TileSize / 2f;
        float tileCenterY = Posicao.Y + (EstaMadura() ? TileSize / 1.5f : TileSize / 2f);

        float spriteAnchorX = FrameWidth / 2f;
        float spriteAnchorY = FrameHeight;

        float drawX = tileCenterX - spriteAnchorX;
        float drawY = tileCenterY - spriteAnchorY;

        Rectangle destination = new(
            (int)MathF.Round(drawX + DrawOffset.X),
            (int)MathF.Round(drawY + DrawOffset.Y),
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

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        return new Rectangle(
            (int)posicao.X,
            (int)posicao.Y,
            TileSize,
            TileSize
        );
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _timerVerificacao += gameTime.ElapsedGameTime.TotalSeconds;

        if (_timerVerificacao < 1)
            return;

        _timerVerificacao = 0;

        AtualizarCrescimento();
    }

    protected void AtualizarCrescimento()
    {
        double agora = Context.State.TempoJogoSegundos;
        double segundosPassados = agora - PlantadoEmTempoJogo;

        if (segundosPassados < 0)
            segundosPassados = 0;

        int novoEstagio = (int)(segundosPassados / SegundosPorEstagio);
        novoEstagio = Math.Clamp(novoEstagio, 0, EstagioMaximo);

        if (novoEstagio == EstagioAtual)
            return;

        EstagioAtual = novoEstagio;
        InfoPlantacao["estagio"] = EstagioAtual.ToString();

        AtualizarSpritePorEstagio();
        AtualizaSolo();
    }

    public void CarregarData()
    {
        if (InfoPlantacao.TryGetValue("estagio", out string estagioString) &&
            int.TryParse(estagioString, out int estagio))
        {
            EstagioAtual = Math.Clamp(estagio, 0, EstagioMaximo);
        }

        if (InfoPlantacao.TryGetValue("plantadoEmTempoJogo", out string plantadoEmTempoJogoString) &&
            double.TryParse(plantadoEmTempoJogoString, out double plantadoEmTempoJogo))
        {
            PlantadoEmTempoJogo = plantadoEmTempoJogo;
        }
        else
        {
            PlantadoEmTempoJogo = Context.State.TempoJogoSegundos - (EstagioAtual * SegundosPorEstagio);
            InfoPlantacao["plantadoEmTempoJogo"] = PlantadoEmTempoJogo.ToString();
        }

        AtualizarSpritePorEstagio();
        AtualizaSolo();
        AtualizarCrescimento();
    }

    protected abstract void AtualizarSpritePorEstagio();

    public bool EstaMadura()
    {
        return EstagioAtual >= EstagioMaximo;
    }

    public void SetSolo(Entity solo)
    {
        Solo = solo;
    }

    public void AtualizaSolo()
    {
        if (EstagioAtual < EstagioMaximo)
        {
            Solo?.AtualizarSprite("Soil02");
            IsColhivel = false;
        }
        else
        {
            Solo?.AtualizarSprite("Soil03");
            IsColhivel = true;
        }
    }
}