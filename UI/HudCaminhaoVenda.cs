using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Entities.WorldObjects.Interactables;

namespace thegame.UI;

public class HudCaminhaoVenda(GameContext context) : BaseHud(context)
{
    private const int QtdSlotsCaminhao = 24;
    private const int ColunasSlots = 8;

    private readonly Texture2D _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");

    private Rectangle CaixaHud;
    private Rectangle AreaItensCaminhao;
    private Rectangle AreaItensBag;

    private readonly List<ItemStackSave> CaminhaoItens = [];

    private Vector2 destTextoDinheiro;
    private Vector2 destBotaoEnviar;
    private Vector2 destBotaoReceber;
    private Rectangle BotaoEnviar;
    private Rectangle BotaoReceber;

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Context.UIState.VendaAberta)
            return;

        CalcularLayout();
        GarantirSlotsCaminhao();

        int sliceSize = 8;

        DrawNineSlice(spriteBatch, _layoutUiTexture, CaixaHud, sliceSize);

        DrawItensCaminhao(spriteBatch);
        CaminhaoVendaState state = Context.State.Venda;

        BotaoEnviar = DrawButton(
                spriteBatch,
                destBotaoEnviar,
                "Enviar",
                new Color(90, 60, 35),
                state.Status == CaminhaoVendaStatus.Disponivel,
                Color.Black,
                0.7f
            );

        BotaoReceber = DrawButton(
                spriteBatch,
                destBotaoReceber,
                "Receber",
                new Color(90, 60, 35),
                state.Status == CaminhaoVendaStatus.AguardandoColeta,
                Color.Black,
                0.7f
            );

        DrawTextOutlined(
            spriteBatch,
            $"R$ {Context.State.Venda.DinheiroPendente}",
            destTextoDinheiro,
            Color.White,
            0.6f
        );

        DrawItensBag(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        if (!Context.UIState.VendaAberta)
            return;

        CalcularLayout();
        GarantirSlotsCaminhao();

        if (Context.Input.IsRightClickPressed())
        {
            MouseState mouse = Mouse.GetState();
            bool clickNaParteDoCaminhao = mouse.Y < AreaItensBag.Top;
            if (clickNaParteDoCaminhao)
                VerificarClickCompra();
            else
                VerificarClickVenda();
        }

        if (Context.Input.WasClicked(BotaoEnviar))
        {
            CaminhaoVenda caminhaoVenda = Context.UIState.EntityEmFoco as CaminhaoVenda;
            caminhaoVenda.EnviarParaVenda();
        }

        if (Context.Input.WasClicked(BotaoReceber))
        {
            CaminhaoVenda caminhaoVenda = Context.UIState.EntityEmFoco as CaminhaoVenda;
            caminhaoVenda.RecolherVenda();
        }

    }

    private void CalcularLayout()
    {
        int tamanhoX = 590;
        int tamanhoY = 550;

        int x = screenWidth / 2 - tamanhoX / 2;
        int y = screenHeight / 2 - tamanhoY / 2;

        CaixaHud = new Rectangle(x, y, tamanhoX, tamanhoY);

        AreaItensCaminhao = new Rectangle(
            CaixaHud.X,
            CaixaHud.Y,
            CaixaHud.Width,
            CaixaHud.Height
        );

        AreaItensBag = new Rectangle(
            CaixaHud.X,
            CaixaHud.Y + CaixaHud.Height / 2 - 10,
            CaixaHud.Width,
            CaixaHud.Height / 2
        );

        destTextoDinheiro = new Vector2(
            CaixaHud.X + 25,
            AreaItensBag.Y
        );

        destBotaoEnviar = new Vector2(
            AreaItensBag.Right - 125,
            AreaItensBag.Y - 15
        );

        destBotaoReceber = new Vector2(
            AreaItensBag.Right - 250,
            AreaItensBag.Y - 15
        );
    }

    private void GarantirSlotsCaminhao()
    {
        while (CaminhaoItens.Count < QtdSlotsCaminhao)
            CaminhaoItens.Add(null);
    }

    private void DrawItensCaminhao(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < QtdSlotsCaminhao; i++)
        {
            Rectangle slot = GetBagSlotRectangle(AreaItensCaminhao, ColunasSlots, i);

            ItemStackSave item = CaminhaoItens[i];

            DrawSlot(spriteBatch, slot, item);
        }
    }

    private void DrawItensBag(SpriteBatch spriteBatch)
    {
        int limiteBag = gameState.Inventory.GetLimiteItensBag;

        for (int i = 0; i < limiteBag; i++)
        {
            Rectangle slot = GetBagSlotRectangle(AreaItensBag, ColunasSlots, i);

            ItemStackSave item = null;

            if (gameState.Inventory.Itens.Count > i)
                item = gameState.Inventory.Itens[i];

            DrawSlot(spriteBatch, slot, item);
        }
    }

    private void DrawSlot(SpriteBatch spriteBatch, Rectangle slot, ItemStackSave item)
    {
        spriteBatch.Draw(_slotTexture, slot, Color.White);

        if (item == null)
            return;

        if (string.IsNullOrWhiteSpace(item.ItemId))
            return;

        var result = TryGetItemTexture(item.ItemId);

        if (!result.HasValue)
            return;

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

    private void VerificarClickCompra()
    {
        for (int i = 0; i < CaminhaoItens.Count; i++)
        {
            ItemStackSave produto = CaminhaoItens[i];

            Rectangle slot = GetBagSlotRectangle(AreaItensCaminhao, ColunasSlots, i);

            if (Context.Input.WasRightClicked(slot))
            {
                if (string.IsNullOrWhiteSpace(produto?.ItemId))
                    return;

                Compra(produto);
                return;
            }
        }
    }

    private void VerificarClickVenda()
    {
        int limiteBag = gameState.Inventory.GetLimiteItensBag;

        for (int i = 0; i < limiteBag; i++)
        {
            if (gameState.Inventory.Itens.Count <= i)
                continue;

            ItemStackSave produto = gameState.Inventory.Itens[i];

            Rectangle slot = GetBagSlotRectangle(AreaItensBag, ColunasSlots, i);

            if (Context.Input.WasRightClicked(slot))
            {
                if (string.IsNullOrWhiteSpace(produto?.ItemId))
                    return;

                Venda(produto);
                return;
            }
        }
    }

    private void Compra(ItemStackSave item)
    {
        int quantidade = 1;

        bool compra = Mercado.ComprarItem(context, item.ItemId, quantidade);

        if (!compra)
            return;

        int index = CaminhaoItens.FindIndex(e => e == item);

        if (index < 0)
            return;

        item.Quantidade -= quantidade;

        if (item.Quantidade <= 0)
            CaminhaoItens[index] = null;
    }

    private void Venda(ItemStackSave item)
    {
        int quantidade = 1;

        bool venda = Mercado.VenderItem(context, item.ItemId, quantidade);

        if (!venda)
            return;

        ItemStackSave existente = CaminhaoItens.FirstOrDefault(e => e?.ItemId == item.ItemId);

        if (existente != null)
        {
            existente.Quantidade += quantidade;
            return;
        }

        int indexLivre = CaminhaoItens.FindIndex(e => e == null || string.IsNullOrWhiteSpace(e.ItemId));

        if (indexLivre < 0)
            return;

        ItemStackSave clone = Context.ClonarItem(item);
        clone.Quantidade = quantidade;

        CaminhaoItens[indexLivre] = clone;
    }
}