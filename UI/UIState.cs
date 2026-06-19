using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities.Npcs;

namespace thegame.UI;

public class UIState
{
    public bool MenuAberto { get; private set; }
    public bool LojaAberta { get; private set; }
    public bool BagAberta { get; private set; }
    public bool BauAberto { get; private set; }
    public Texture2D LojaSprite { get; private set; }
    public List<LojaItens> ListProdutosLoja { get; private set; } = [];

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

    public void ToggleLoja(GameContext context, Guid lojaId, string spriteId, List<LojaItens> produtos)
    {
        if (LojaAberta && _lojaDonoId == lojaId)
        {
            FecharLoja();
            return;
        }

        LojaAberta = true;
        _lojaDonoId = lojaId;
        ListProdutosLoja = produtos;
        LojaSprite = NpcTexture2D.GetNpcPortraitById(context, spriteId);
    }

    public void FecharLoja()
    {
        LojaAberta = false;
        LojaSprite = null;
        ListProdutosLoja = [];
        _lojaDonoId = null;
    }

    public void ResetarTudo()
    {
        MenuAberto = false;
        LojaAberta = false;
        BagAberta = false;
        BauAberto = false;
        LojaSprite = null;
        ListProdutosLoja = [];
        _lojaDonoId = null;
    }
}