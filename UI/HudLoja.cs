using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.UI;

public class HudLoja(GameContext context) : BaseHud(context)
{
    private readonly Texture2D _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");
    private Rectangle CaixaHud;
    private int InicioDesenhoItensLoja = 0;

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Context.UIState.LojaAberta)
        {
            int tamanhoX = 590;
            int tamanhoY = 550;
            int width = screenWidth / 2 - tamanhoX / 2;
            int height = screenHeight / 2 - tamanhoY / 2;

            int sliceSize = 8;
            int limit = gameState.Inventory.GetLimiteItensBag;

            Rectangle dest = new(width, height, tamanhoX, tamanhoY);
            CaixaHud = dest;
            InicioDesenhoItensLoja = dest.X + dest.Width / 4;

            DrawNineSlice(spriteBatch, _layoutUiTexture, dest, sliceSize);
            DrawSpriteNpc(spriteBatch);
            DrawProdutos(spriteBatch);
            DrawTextOutlined(spriteBatch, $"R$ {Context.State.Player.Carteira}", new(dest.X + 25, dest.Y * 2), Color.White, 0.6f);
            DrawBagSlots(spriteBatch, new(
                dest.X,
                dest.Y * 2,
                dest.Width,
                dest.Height
            ), 8, limit);
        }
    }

    private void DrawSpriteNpc(SpriteBatch spriteBatch)
    {
        Texture2D sprite = Context.UIState.LojaSprite;
        Rectangle dest = new(
            CaixaHud.X,
            CaixaHud.Y,
            165, 153
        );
        spriteBatch.Draw(sprite, dest, Color.White);

        string fala = TratarWidthFala(Context.UIState.LojaFala, 165, 0.5f);

        DrawTextOutlined(spriteBatch, fala, new(dest.X + 25, dest.Bottom + 5), Color.White, 0.5f);
    }

    private string TratarWidthFala(string fala, float larguraMaxima, float scale)
    {
        if (string.IsNullOrWhiteSpace(fala))
            return "";

        string[] palavras = fala.Split(' ');
        string linhaAtual = "";
        List<string> linhas = [];

        foreach (string palavra in palavras)
        {
            string testeLinha;

            if (string.IsNullOrEmpty(linhaAtual))
                testeLinha = palavra;
            else
                testeLinha = linhaAtual + " " + palavra;

            float larguraTexto = fonte.MeasureString(testeLinha).X * scale;

            if (larguraTexto <= larguraMaxima)
            {
                linhaAtual = testeLinha;
            }
            else
            {
                if (!string.IsNullOrEmpty(linhaAtual))
                    linhas.Add(linhaAtual);

                linhaAtual = palavra;
            }
        }

        if (!string.IsNullOrEmpty(linhaAtual))
            linhas.Add(linhaAtual);

        return string.Join("\n", linhas);
    }

    private void DrawProdutos(SpriteBatch spriteBatch)
    {
        List<LojaItens> ListProdutosLoja = context.UIState.ListProdutosLoja;

        for (int i = 0; i < ListProdutosLoja.Count; i++)
        {
            LojaItens produtos = ListProdutosLoja[i];
            CaixaHud.X = InicioDesenhoItensLoja;
            Rectangle slot = GetBagSlotRectangle(CaixaHud, 8, i);
            spriteBatch.Draw(_slotTexture, slot, Color.White);

            if (Context.Input.WasClicked(slot))
                Mercado.Comprar(context, produtos.ItemId, 1);

            // busca a  textura da imagem para desenhar no retangulo
            var result = TryGetItemTexture(produtos.ItemId);

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

            string Preco = $"R$ {produtos.Preco}";
            Vector2 textPosition = new(
                itemRect.X,
                itemRect.Y + itemRect.Height + 15
            );

            DrawTextOutlined(spriteBatch, Preco, textPosition, Color.White, 0.6f);
        }
    }

    private void DrawBagSlots(SpriteBatch spriteBatch, Rectangle bag, int columns, int itens)
    {
        List<ItemStackSave> items = gameState.Inventory.Itens;

        for (int i = 0; i < itens; i++)
        {
            Rectangle slot = GetBagSlotRectangle(bag, columns, i);
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

    public override void Update(GameTime gameTime)
    {
        if (!Context.UIState.LojaAberta)
            return;

        VerificarClickProdutos();
    }

    private void VerificarClickProdutos()
    {
        List<LojaItens> produtosLoja = Context.UIState.ListProdutosLoja;

        for (int i = 0; i < produtosLoja.Count; i++)
        {
            LojaItens produto = produtosLoja[i];

            CaixaHud.X = InicioDesenhoItensLoja;

            Rectangle slot = GetBagSlotRectangle(CaixaHud, 8, i);

            if (Context.Input.WasRightClicked(slot))
            {
                if (string.IsNullOrWhiteSpace(produto.ItemId))
                    return;

                Mercado.Comprar(Context, produto.ItemId, 1);
                return;
            }
        }
    }
}