using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Solo.Plantacoes;

public abstract class Plantacao : Entity
{
    protected int EstagioAtual = 0;
    protected int EstagioMaximo = 3;
    protected int SegundosPorEstagio = 30;

    protected long PlantadoEmUnix;

    private double _timerVerificacao;
    public Dictionary<string, string> InfoPlantacao = [];

    protected Plantacao(GameContext context, string entityId, Vector2 posicao) : base(context, entityId, posicao)
    {
        BloqueiaMovimento = false;

        PlantadoEmUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        InfoPlantacao["PlantadoEm"] = PlantadoEmUnix.ToString();
        InfoPlantacao["Estagio"] = EstagioAtual.ToString();

        // AtualizarSpritePorEstagio();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _timerVerificacao += gameTime.ElapsedGameTime.TotalSeconds;

        // Não precisa verificar todo frame.
        if (_timerVerificacao < 1)
            return;

        _timerVerificacao = 0;

        AtualizarCrescimento();
    }

    protected void AtualizarCrescimento()
    {
        long agora = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long segundosPassados = agora - PlantadoEmUnix;
        int novoEstagio = (int)(segundosPassados / SegundosPorEstagio);
        novoEstagio = Math.Clamp(novoEstagio, 0, EstagioMaximo);

        if (novoEstagio == EstagioAtual)
            return;

        EstagioAtual = novoEstagio;
        InfoPlantacao["estagio"] = EstagioAtual.ToString();
        AtualizarSpritePorEstagio();
    }

    public void CarregarData()
    {
        if (InfoPlantacao.TryGetValue("plantadoEm", out string plantadoEmString) &&
            long.TryParse(plantadoEmString, out long plantadoEm))
        {
            PlantadoEmUnix = plantadoEm;
        }
        else
        {
            PlantadoEmUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            InfoPlantacao["plantadoEm"] = PlantadoEmUnix.ToString();
        }

        AtualizarCrescimento();
    }

    protected abstract void AtualizarSpritePorEstagio();

    public bool EstaMadura()
    {
        return EstagioAtual >= EstagioMaximo;
    }
}