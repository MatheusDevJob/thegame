using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Interactables;

public class CaminhaoVenda : Entity
{
    private const string nome = "CaminhaoVenda";
    private const string fala = "CaminhaoVenda";

    private readonly float _scale = 0.7f;
    protected float DistanciaInteracao = 30f;
    protected bool PlayerPerto;
    protected Vector2 Center => Posicao + new Vector2(FrameWidth * _scale / 2f, FrameHeight * _scale / 2f);
    protected bool MostrarFala;
    private const int segundosParaVoltar = 10;
    private string TempoEspera;

    public CaminhaoVenda(GameContext context, Vector2 posicao) : base(context, "CaminhaoVenda", posicao)
    {
        FrameWidth = 64;
        FrameHeight = 64;
        BloqueiaMovimento = true;
    }

    private readonly Guid _lojaId = Guid.NewGuid();

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        bool lojaPertence = Context.UIState.VendaPertenceAo(this);
        bool jogadorLonge = IsPlayerFartherThanMe(2);

        System.Diagnostics.Debug.WriteLine(
            $"LojaPertence: {lojaPertence} | JogadorLonge: {jogadorLonge} | LojaId: {_lojaId}"
        );

        if (lojaPertence && jogadorLonge)
        {
            Context.UIState.ResetarTudo();
        }

        CalcularTempoEspera();

        CaminhaoVoltou();
    }

    private void CalcularTempoEspera()
    {
        CaminhaoVendaState state = Context.State.Venda;
        if (state.Status == CaminhaoVendaStatus.EmRota)
        {
            double agora = Context.State.TempoJogoSegundos;
            double retorno = state.RetornaEmTempoJogo;

            if (agora >= retorno)
            {
                state.Status = CaminhaoVendaStatus.AguardandoColeta;
            }
            TempoEspera = Context.FormatarTempoEspera(retorno - agora);
        }
        else if (state.Status == CaminhaoVendaStatus.AguardandoColeta)
        {
            TempoEspera = "$$$";
        }
        else if (state.Status == CaminhaoVendaStatus.Disponivel)
        {
            TempoEspera = "E";
        }
    }

    public virtual void UpdateInteraction(Player player)
    {
        float distancia = Vector2.Distance(Center, player.Center);
        PlayerPerto = distancia <= DistanciaInteracao;

        if (!PlayerPerto)
        {
            MostrarFala = false;
            return;
        }

        if (Context.Input.IsKeyPressed(Keys.E))
            NpcAction();
    }

    protected void NpcAction()
    {
        CaminhaoVendaState state = Context.State.Venda;
        if (state.Status != CaminhaoVendaStatus.EmRota)
            Context.UIState.ToggleVenda(this, _lojaId, fala);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        CaminhaoVendaState state = Context.State.Venda;
        Vector2 position = new(Center.X, Posicao.Y - 16);

        if (PlayerPerto && !MostrarFala)
        {
            Context.UI.DrawKeyHint(spriteBatch, TempoEspera, position);
        }
        else if (state.Status != CaminhaoVendaStatus.Disponivel)
        {
            Context.UI.DrawKeyHint(spriteBatch, TempoEspera, position);
        }

        if (MostrarFala)
            Context.UI.DrawNpcSpeech(spriteBatch, fala, Center, Posicao.Y);

    }

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        return new Rectangle(
            (int)posicao.X + 21,
            (int)posicao.Y + 10,
            21,
            41
        );
    }

    public void EnviarParaVenda()
    {
        CaminhaoVendaState state = Context.State.Venda;
        if (state.Status == CaminhaoVendaStatus.Disponivel)
        {
            Context.UIState.ToggleVenda(this, _lojaId, fala);
            state.ItensEnviados = [];
            double agora = Context.State.TempoJogoSegundos;

            state.Status = CaminhaoVendaStatus.EmRota;
            state.RetornaEmTempoJogo = agora + segundosParaVoltar;
        }
    }

    public void RecolherVenda()
    {
        CaminhaoVendaState state = Context.State.Venda;
        if (state.Status == CaminhaoVendaStatus.AguardandoColeta)
        {
            Context.State.Player.Carteira += state.DinheiroPendente;
            state.DinheiroPendente = 0;
            state.Status = CaminhaoVendaStatus.Disponivel;
        }
    }

    private void CaminhaoVoltou()
    {
        CaminhaoVendaState state = Context.State.Venda;
        if (state.Status == CaminhaoVendaStatus.EmRota)
        {
            double agora = Context.State.TempoJogoSegundos;
            double retorno = state.RetornaEmTempoJogo;

            if (agora >= retorno)
            {
                state.Status = CaminhaoVendaStatus.AguardandoColeta;
            }
        }
    }
}