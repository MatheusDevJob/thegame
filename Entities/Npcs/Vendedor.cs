using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Npcs;

public class Vendedor(GameContext context, string nome, string fala, Vector2 posicao, List<LojaItens> lojaItens) : Npc(context, nome, fala, posicao)
{

    private readonly Guid _lojaId = Guid.NewGuid();

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Context.UIState.LojaPertenceAo(_lojaId) && IsPlayerFartherThanMe(2))
        {
            Context.UIState.ResetarTudo();
        }
    }

    protected override void NpcAction()
    {
        Context.UIState.ToggleLoja(Context, _lojaId, nome, fala, lojaItens);
    }
}