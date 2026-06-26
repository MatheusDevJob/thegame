using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Npcs;

namespace thegame.UI;

public class UIState
{
    public bool MenuAberto { get; private set; }
    public bool LojaAberta { get; private set; }
    public bool VendaAberta { get; private set; }
    public bool BagAberta { get; private set; }
    public bool BauAberto { get; private set; }
    public Texture2D LojaSprite { get; private set; }
    public string LojaFala { get; private set; }
    public List<LojaItens> ListProdutosLoja { get; private set; } = [];
    public Entity EntityEmFoco { get; private set; }

    public void SetMenuAberto(bool StatusMenu)
    {
        MenuAberto = StatusMenu;
    }

    public void ToogleBag()
    {
        BagAberta = !BagAberta;
    }

    private Guid? _lojaDonoId;

    public bool LojaPertenceAo(Guid lojaId)
    {
        return LojaAberta && _lojaDonoId == lojaId;
    }

    public bool VendaPertenceAo(Entity entity)
    {
        return VendaAberta && EntityEmFoco == entity;
    }

    public void ToggleLoja(GameContext context, Guid lojaId, string spriteId, string fala, List<LojaItens> produtos)
    {
        if (LojaAberta && _lojaDonoId == lojaId)
        {
            ResetarTudo();
            return;
        }

        LojaFala = fala;
        LojaAberta = true;
        _lojaDonoId = lojaId;
        ListProdutosLoja = produtos;
        LojaSprite = NpcTexture2D.GetNpcPortraitById(context, spriteId);
    }

    public void ToggleVenda(Entity entity, Guid lojaId, string fala)
    {
        if (VendaAberta && _lojaDonoId == lojaId)
        {
            ResetarTudo();
            return;
        }

        EntityEmFoco = entity;
        LojaFala = fala;
        VendaAberta = true;
        _lojaDonoId = lojaId;
    }

    public void ResetarTudo()
    {
        MenuAberto = false;
        LojaAberta = false;
        EntityEmFoco = null;
        VendaAberta = false;
        BagAberta = false;
        BauAberto = false;
        LojaSprite = null;
        ListProdutosLoja = [];
        _lojaDonoId = null;
    }
}