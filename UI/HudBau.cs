using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Entities.WorldObjects.Interactables;

namespace thegame.UI;

public class HudBau(GameContext context) : BaseHud(context)
{
    private readonly Texture2D _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");
    // espaço da divisória do baú, usado para verificar se o click foi acima ou abaixo dela (acima baú, abaixo bag)
    private Rectangle destLinhaHorizontal;
    private Rectangle CaixaHudBau;
    private Rectangle CaixaHudBag;
    private bool BauAberto;
    private Bau bau;

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (BauAberto)
        {
            int tamanhoX = 600;
            int tamanhoY = 550;
            int width = screenWidth / 2 - tamanhoX / 2;
            int height = screenHeight / 2 - tamanhoY / 2;

            int sliceSize = 8;
            int limit = gameState.Inventory.GetLimiteItensBag;

            bau = gameState.EntidadeEmFoco as Bau;

            Rectangle dest = new(width, height, tamanhoX, tamanhoY);
            CaixaHudBau = dest;

            DrawNineSlice(spriteBatch, nineSlice, dest, sliceSize);

            DrawItensBau(spriteBatch, dest);

            DrawLinhaHorizontal(spriteBatch, dest);

            CaixaHudBag = new(
                dest.X,
                dest.Y * 2,
                dest.Width,
                dest.Height
            );
            DrawBagSlots(spriteBatch, CaixaHudBag, 8, limit);
            MouseMovendoItem(spriteBatch);
        }
    }

    private void DrawLinhaHorizontal(SpriteBatch spriteBatch, Rectangle dest)
    {
        destLinhaHorizontal = new(
            dest.X + 5,
            dest.Y * 2 - 20,
            dest.Width - 10,
            5
        );

        spriteBatch.Draw(_pixel, destLinhaHorizontal, Color.White);
    }

    private void DrawItensBau(SpriteBatch spriteBatch, Rectangle bag)
    {
        Bau bau = gameState.EntidadeEmFoco as Bau;
        List<ItemStackSave> items = bau.Items;
        int QtdSlots = bau.QtdSlots;

        for (int i = 0; i < QtdSlots; i++)
        {
            Rectangle slot = GetBagSlotRectangle(bag, 8, QtdSlots, i);
            spriteBatch.Draw(_slotTexture, slot, Color.White);

            if (items.Count <= i)
                continue;

            ItemStackSave item = items.FirstOrDefault((item) => item?.ListIndex == i);

            if (item == null)
                continue;

            // busca a  textura da imagem para desenhar no retangulo
            var result = TryGetItemTexture(item.ItemId);

            if (!result.HasValue)
                continue;

            Rectangle itemRect = new(
                slot.X + 10,
                slot.Y + 8,
                slot.Width - 20,
                slot.Height - 20
            );
            Texture2D texture = result.Value.Item1;
            Rectangle source = result.Value.Item2;

            spriteBatch.Draw(texture, itemRect, source, Color.White);

            string quantidade = item.Quantidade.ToString();
            Vector2 textSize = fonte.MeasureString(quantidade) * 0.8f;
            Vector2 textPosition = new(
                slot.Right - textSize.X - 8,
                slot.Bottom - textSize.Y - 6
            );

            DrawTextOutlined(spriteBatch, quantidade, textPosition, Color.White, 0.8f);
        }
    }

    private void DrawBagSlots(SpriteBatch spriteBatch, Rectangle bag, int columns, int itens)
    {
        List<ItemStackSave> items = gameState.Inventory.Itens;

        for (int i = 0; i < itens; i++)
        {
            Rectangle slot = GetBagSlotRectangle(bag, columns, itens, i);
            spriteBatch.Draw(_slotTexture, slot, Color.White);

            if (items.Count <= i)
                continue;

            ItemStackSave item = items[i];

            if (item == null)
                continue;

            // busca a  textura da imagem para desenhar no retangulo
            var result = TryGetItemTexture(item.ItemId);

            if (!result.HasValue)
                continue;

            Rectangle itemRect = new(
                slot.X + 10,
                slot.Y + 8,
                slot.Width - 20,
                slot.Height - 20
            );
            Texture2D texture = result.Value.Item1;
            Rectangle source = result.Value.Item2;

            spriteBatch.Draw(texture, itemRect, source, Color.White);

            string quantidade = item.Quantidade.ToString();
            Vector2 textSize = fonte.MeasureString(quantidade) * 0.8f;
            Vector2 textPosition = new(
                slot.Right - textSize.X - 8,
                slot.Bottom - textSize.Y - 6
            );

            DrawTextOutlined(spriteBatch, quantidade, textPosition, Color.White, 0.8f);
        }
    }


    /* 
        UPDATE AREA
    */
    private int _selectedBagIndex = -1;
    private int _selectedBauIndex = -1;
    private bool ClickNoBau;
    private List<string> OrdemClicks = [];

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        BauAberto = gameState.EntidadeEmFoco is Bau;
        if (!BauAberto) return;
        UpdateBagClick();
        MouseSetItem();
        CancelarMouseSetItem();
    }

    private void UpdateBagClick()
    {
        if (!BauAberto)
            return;
        if (!Context.Input.IsLeftClickPressed())
            return;

        MouseState mouse = Mouse.GetState();
        int QtdSlots = bau.QtdSlots;
        ClickNoBau = mouse.Y < destLinhaHorizontal.Top;
        if (OrdemClicks.Count > 1)
        {
            CancelarSetItem();
            return;
        }
        // acima é baú
        if (ClickNoBau)
        {
            for (int i = 0; i < QtdSlots; i++)
            {
                Rectangle slot = GetBagSlotRectangle(CaixaHudBau, 8, QtdSlots, i);
                if (!Context.Input.WasClicked(slot))
                    continue;
                _selectedBauIndex = i;
                OrdemClicks.Add("bau");
                return;
            }
        }
        // abaixo é bag
        else
        {
            int limit = gameState.Inventory.GetLimiteItensBag;

            for (int i = 0; i < limit; i++)
            {
                Rectangle slot = GetBagSlotRectangle(CaixaHudBag, 8, limit, i);

                if (!Context.Input.WasClicked(slot))
                    continue;

                _selectedBagIndex = i;
                OrdemClicks.Add("bag");
                return;
            }
        }
    }


    /* 
        EVENTO MOUSE AREA
        controle dos itens pelo mouse
    */
    private Texture2D texture;
    private Rectangle position;
    private ItemStackSave _itemNaMao;
    private int? _slotOrigem;
    private void MouseMovendoItem(SpriteBatch spriteBatch)
    {
        if (texture != null)
        {
            MouseState mouse = Mouse.GetState();

            Rectangle rect = new(
                mouse.X - 24,
                mouse.Y - 24,
                48,
                48
            );
            spriteBatch.Draw(texture, rect, position, Color.White);
        }

        // se nenhum index foi definido, não fazer nada
        if (_selectedBagIndex < 0 && _selectedBauIndex < 0) return;

        // manipular item do baú
        if (_selectedBauIndex >= 0)
        {
            List<ItemStackSave> items = bau.Items;
            if (items.Count < _selectedBauIndex) return;
            if (_itemNaMao != null) return;

            _itemNaMao ??= items[_selectedBauIndex];
            if (_itemNaMao == null) return;
            _slotOrigem = _selectedBauIndex;

            items[_selectedBauIndex] = null;
        }
        // manipular item da bag
        else
        {
            List<ItemStackSave> items = gameState.Inventory.Itens;
            if (items.Count < _selectedBagIndex) return;
            if (_itemNaMao != null) return;

            _itemNaMao ??= items[_selectedBagIndex];
            if (_itemNaMao == null) return;
            _slotOrigem = _selectedBagIndex;

            items[_selectedBagIndex] = null;
        }

        var result = TryGetItemTexture(_itemNaMao?.ItemId);

        if (result.HasValue)
        {
            var (Texture2D, source) = result.Value;
            texture = Texture2D;
            position = source;

        }
    }

    private void CancelarMouseSetItem()
    {
        if (Context.Input.IsRightClickPressed() && _slotOrigem != null)
        {
            CancelarSetItem();
        }
    }

    private void CancelarSetItem()
    {
        if (OrdemClicks[0] == "bau")
            bau.Items[(int)_slotOrigem] = _itemNaMao;
        else
            gameState.Inventory.Itens[(int)_slotOrigem] = _itemNaMao;

        LimparCacheMoverItens();
    }

    private void MouseSetItem()
    {
        if (Context.Input.IsLeftClickPressed() && _slotOrigem != null)
        {
            if (OrdemClicks.Count < 1)
                return;

            if (OrdemClicks[0] == "bag" && OrdemClicks[1] == "bau")
            {
                ItemStackSave existente = bau.Items[_selectedBauIndex];
                if (existente != null) return;
                _itemNaMao.ListIndex = _selectedBauIndex;
                bau.Items[_selectedBauIndex] = _itemNaMao;
            }
            else if (OrdemClicks[0] == "bau" && OrdemClicks[1] == "bag")
            {
                List<ItemStackSave> items = gameState.Inventory.Itens;
                ItemStackSave existente = items[_selectedBagIndex];
                if (existente != null) return;
                _itemNaMao.ListIndex = _selectedBagIndex;
                items[_selectedBagIndex] = _itemNaMao;
            }
            else if (OrdemClicks[0] == "bau" && OrdemClicks[1] == "bau")
            {
                ItemStackSave existente = bau.Items[_selectedBauIndex];

                if (existente != null)
                {
                    existente.ListIndex = (int)_slotOrigem;
                    bau.Items[(int)_slotOrigem] = existente;
                }

                _itemNaMao.ListIndex = _selectedBauIndex;
                bau.Items[_selectedBauIndex] = _itemNaMao;
            }
            else if (OrdemClicks[0] == "bag" && OrdemClicks[1] == "bag")
            {
                List<ItemStackSave> items = gameState.Inventory.Itens;
                ItemStackSave existente = items[_selectedBagIndex];

                if (existente != null)
                {
                    existente.ListIndex = (int)_slotOrigem;
                    items[(int)_slotOrigem] = existente;
                }
                _itemNaMao.ListIndex = _selectedBagIndex;
                items[_selectedBagIndex] = _itemNaMao;
            }

            LimparCacheMoverItens();
        }
    }
    private void LimparCacheMoverItens()
    {
        texture = null;
        _selectedBagIndex = -1;
        _selectedBauIndex = -1;
        _itemNaMao = null;
        _slotOrigem = null;
        OrdemClicks.Clear();
    }
}