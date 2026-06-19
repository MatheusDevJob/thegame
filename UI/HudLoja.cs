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
            int tamanhoY = 350;
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

            string quantidade = produtos.Quantidade.ToString();
            Vector2 textSize = fonte.MeasureString(quantidade) * 0.8f;
            Vector2 textPosition = new(
                slot.Right - textSize.X - 8,
                slot.Bottom - textSize.Y - 6
            );

            DrawTextOutlined(spriteBatch, quantidade, textPosition, Color.White, 0.8f);
        }
    }

    public override void Update(GameTime gameTime) { }
}