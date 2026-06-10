using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;
using thegame.Entities.WorldObjects.Interactables;
using thegame.Items;

namespace thegame.UI;

public class HudBau(GameContext context) : BaseHud(context)
{
    private readonly Texture2D _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");

    public void Draw(SpriteBatch spriteBatch)
    {
        if (gameState.EntidadeEmFoco is Bau)
        {
            int tamanhoX = 600;
            int tamanhoY = 550;
            int width = screenWidth / 2 - tamanhoX / 2;
            int height = screenHeight / 2 - tamanhoY / 2;
            int sliceSize = 8;
            int limit = gameState.Inventory.GetLimiteItensBag;

            Rectangle dest = new(width, height, tamanhoX, tamanhoY);

            DrawNineSlice(spriteBatch, nineSlice, dest, sliceSize);

            DrawItensBau(spriteBatch, dest);

            DrawLinhaHorizontal(spriteBatch, dest);

            Rectangle initSlots = new(
                dest.X,
                dest.Y * 2,
                dest.Width,
                dest.Height
            );
            DrawBagSlots(spriteBatch, initSlots, 8, limit);
        }
    }

    private void DrawLinhaHorizontal(SpriteBatch spriteBatch, Rectangle dest)
    {
        Rectangle destLinhaHorizontal = new(
            dest.X + 5,
            dest.Y * 2 - 20,
            dest.Width - 10,
            5
        );

        spriteBatch.Draw(_pixel, destLinhaHorizontal, Color.White);
    }

    private void DrawItensBau(SpriteBatch spriteBatch, Rectangle bag)
    {
        List<ItemStackSave> items = gameState.EntidadeEmFoco.Data;
        Bau bau = gameState.EntidadeEmFoco as Bau;
        int QtdSlots = bau.QtdSlots;

        for (int i = 0; i < QtdSlots; i++)
        {
            Rectangle slot = GetBagSlotRectangle(bag, 8, QtdSlots, i);
            spriteBatch.Draw(_slotTexture, slot, Color.White);

            if (items.Count <= i)
                continue;

            ItemStackSave item = items.FirstOrDefault((item) => item.ListIndex == i);

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
}