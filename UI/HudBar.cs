using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Entities;
using thegame.Items;
using thegame.Maps;

namespace thegame.UI;

public class HudBar(GameContext context) : BaseHud(context)
{
    private readonly Texture2D _hudBoxItem = context.Content.Load<Texture2D>("UI/Hud/itemdisc_01");
    private readonly Texture2D _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");
    private readonly InputManager inputManager = context.Input;
    private readonly int _scale = 3;


    public readonly List<string> boxes = ["AxeTool", "PickaxeTool", "ShovelTool"];
    private const int HotbarSlots = 8;
    private const int SlotSize = 60;
    private const int SlotGap = 0;
    private ItemStack _itemNaMao;
    private int? _slotOrigem;

    private Rectangle bag;
    private int _selectedBagIndex = -1;
    private int _selectedHotbarIndex = -1;

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawCaixaItens(spriteBatch);
        DrawItensOnBar(spriteBatch);
        MouseMovendoItem(spriteBatch);
    }

    public void DrawBag(SpriteBatch spriteBatch)
    {
        bag = GetBagRectangle();
        int limit = gameState.Inventory.GetLimiteItensBag;

        var (leftArea, rightArea) = DrawNineSliceSplit(spriteBatch, _layoutUiTexture, bag, 16, 0.60f);
        DrawBagSlots(spriteBatch, bag, 8, limit);
        DrawBagSlotsHorizontalDivider(spriteBatch, bag, 8, limit, 16);
    }

    public void Update(GameTime gameTime)
    {
        UpdateBagClick();
        UpdateHotbarClick();
        UpdateBarraAcoesClicada();

        if (gameState.LayoutBag && _selectedHotbarIndex >= 0 && (inputManager.IsKeyPressed(Keys.Delete) || inputManager.IsKeyPressed(Keys.Back)))
            ClearHotbarSlot(_selectedHotbarIndex);

        CancelarMouseSetItem();
        MouseSetItem();
    }


    private void UpdateBagClick()
    {
        if (!gameState.LayoutBag)
            return;

        Rectangle bagRect = GetBagRectangle();
        int limit = gameState.Inventory.GetLimiteItensBag;

        for (int i = 0; i < limit; i++)
        {
            Rectangle slot = GetBagSlotRectangle(bagRect, 8, limit, i);

            if (!Context.Input.WasClicked(slot))
                continue;

            _selectedBagIndex = i;
            return;
        }
    }

    private void UpdateHotbarClick()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            Rectangle slot = GetHotbarSlotRectangle(i);

            if (!Context.Input.WasClicked(slot))
                continue;

            _selectedHotbarIndex = i;

            if (gameState.LayoutBag && TryAssignSelectedBagItemToHotbar(i))
                return;

            return;
        }
    }

    private void UpdateBarraAcoesClicada()
    {
        int slotIndex = GetPressedHotbarIndex();

        if (slotIndex < 0)
            return;

        _selectedHotbarIndex = slotIndex;

        if (gameState.LayoutBag && TryAssignSelectedBagItemToHotbar(slotIndex))
            return;

        EquipHotbarSlot(slotIndex);
    }

    private int GetPressedHotbarIndex()
    {
        if (inputManager.IsKeyPressed(Keys.D1)) return 0;
        if (inputManager.IsKeyPressed(Keys.D2)) return 1;
        if (inputManager.IsKeyPressed(Keys.D3)) return 2;
        if (inputManager.IsKeyPressed(Keys.D4)) return 3;
        if (inputManager.IsKeyPressed(Keys.D5)) return 4;
        if (inputManager.IsKeyPressed(Keys.D6)) return 5;
        if (inputManager.IsKeyPressed(Keys.D7)) return 6;
        if (inputManager.IsKeyPressed(Keys.D8)) return 7;
        return -1;
    }

    private bool TryAssignSelectedBagItemToHotbar(int hotbarIndex)
    {
        if (_selectedBagIndex < 0)
            return false;

        List<ItemStack> items = gameState.Inventory.Itens;

        if (_selectedBagIndex >= items.Count)
            return false;

        ItemStack item = items[_selectedBagIndex];

        if (item == null || string.IsNullOrWhiteSpace(item.Id))
            return false;

        if (!CanUseAsHotbarItem(item.Id))
            return false;

        SetHotbarSlot(hotbarIndex, item.Id);
        EquipHotbarSlot(hotbarIndex);
        return true;
    }

    private void EquipHotbarSlot(int slotIndex)
    {
        string itemId = GetHotbarSlot(slotIndex);
        gameState.SetActiveEquipe(itemId);
    }

    private bool CanUseAsHotbarItem(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
            return false;

        var result = TryGetItemTexture(itemId);
        return result.HasValue;
    }

    private void DrawCaixaItens(SpriteBatch spriteBatch)
    {
        // for (int i = 0; i < boxes.Count; i++)
        for (int i = 0; i < 8; i++)
        {
            Rectangle slot = GetHotbarSlotRectangle(i);
            spriteBatch.Draw(_hudBoxItem, slot, Color.White);

            string number = (i + 1).ToString();
            DrawTextOutlined(spriteBatch, number, new Vector2(slot.X + 5, slot.Y + 4), Color.White, 0.55f);

            if (i == _selectedHotbarIndex)
                DrawBorder(spriteBatch, slot, new Color(255, 230, 120), 2);
        }
    }

    private void DrawItensOnBar(SpriteBatch spriteBatch)
    {
        List<ItemStack> items = gameState.Inventory.Itens;
        for (int i = 0; i < 8; i++)
        {
            ItemStack item = items[i];
            if (item == null) continue;

            var result = TryGetItemTexture(item.Id);

            if (!result.HasValue)
                continue;
            Texture2D texture = result.Value.Item1;
            Rectangle source = result.Value.Item2;

            Rectangle slot = GetHotbarSlotRectangle(i);
            Rectangle itemRect = new(
                slot.X + 8,
                slot.Y + 8,
                slot.Width - 16,
                slot.Height - 16
            );

            spriteBatch.Draw(texture, itemRect, source, Color.White);
        }
    }

    private void DrawBagSlots(SpriteBatch spriteBatch, Rectangle bag, int columns, int itens)
    {
        List<ItemStack> items = gameState.Inventory.Itens;

        for (int i = 0; i < itens; i++)
        {
            Rectangle slot = GetBagSlotRectangle(bag, columns, itens, i);
            spriteBatch.Draw(_slotTexture, slot, Color.White);

            // if (i == _selectedBagIndex)
            //     DrawBorder(spriteBatch, slot, new Color(255, 230, 120), 2);

            if (items.Count <= i)
                continue;

            ItemStack item = items[i];

            if (item == null)
                continue;

            // busca a  textura da imagem para desenhar no retangulo
            var result = TryGetItemTexture(item.Id);

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

    private void DrawBagSlotsHorizontalDivider(SpriteBatch spriteBatch, Rectangle bag, int columns, int itens, int sliceSize)
    {
        if (itens <= columns)
            return;

        Rectangle firstSlot = GetBagSlotRectangle(bag, columns, itens, 0);
        Rectangle lastSlotFirstRow = GetBagSlotRectangle(bag, columns, itens, columns - 1);
        Rectangle firstSlotSecondRow = GetBagSlotRectangle(bag, columns, itens, columns);

        int dividerHeight = sliceSize;
        int dividerX = firstSlot.X;
        int dividerWidth = lastSlotFirstRow.Right - firstSlot.X;
        int emptySpaceStart = firstSlot.Bottom;
        int emptySpaceEnd = firstSlotSecondRow.Y;
        int dividerY = emptySpaceStart + ((emptySpaceEnd - emptySpaceStart) / 2) - (dividerHeight / 2);

        Rectangle srcDivider = new(
            sliceSize,
            0,
            _layoutUiTexture.Width - sliceSize * 2,
            0
        );

        Rectangle dstDivider = new(
            dividerX,
            dividerY + 5,
            dividerWidth,
            dividerHeight
        );

        spriteBatch.Draw(_layoutUiTexture, dstDivider, srcDivider, Color.White);
    }

    private Rectangle GetBagRectangle()
    {
        Viewport viewport = Context.GraphicsDevice.Viewport;

        return new Rectangle(
            450,
            250,
            viewport.Width - 900,
            viewport.Height - 500
        );
    }

    private Rectangle GetHotbarSlotRectangle(int index)
    {
        Viewport viewport = Context.GraphicsDevice.Viewport;
        int itemWidth = 18 * _scale;
        int itemHeight = 19 * _scale;
        int startY = viewport.Height - itemHeight - 10;

        int x = viewport.Width - 70;
        int y = startY - (HotbarSlots - 1 - index) * (SlotSize + SlotGap);
        return new Rectangle(x, y, itemWidth, itemHeight);
    }

    private string GetHotbarSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= 8)
            return "";
        List<ItemStack> items = gameState.Inventory.Itens;
        ItemStack item = items[slotIndex];
        return item?.Id ?? "";
    }

    private void SetHotbarSlot(int slotIndex, string itemId)
    {
        gameState.PlayerSave.ActiveEquipe = itemId ?? "";
    }

    private void ClearHotbarSlot(int slotIndex)
    {
        SetHotbarSlot(slotIndex, "");

        if (_selectedHotbarIndex == slotIndex)
            gameState.ActiveEquipe = null;
    }

    private void DrawBorder(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
    {
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
    }

    private static (Rectangle leftArea, Rectangle rightArea) DrawNineSliceSplit(SpriteBatch spriteBatch, Texture2D texture, Rectangle dest, int sliceSize, float leftPercent = 0.60f)
    {
        DrawNineSlice(spriteBatch, texture, dest, sliceSize);

        int dividerWidth = sliceSize;
        int dividerX = dest.X + (int)(dest.Width * leftPercent) - dividerWidth / 2;

        Rectangle srcDivider = new(
            0,
            sliceSize,
            sliceSize,
            texture.Height - sliceSize * 2
        );

        Rectangle dstDivider = new(
            dividerX,
            dest.Y + sliceSize,
            dividerWidth,
            dest.Height - sliceSize * 2
        );

        spriteBatch.Draw(texture, dstDivider, srcDivider, Color.White);

        Rectangle leftArea = new(
            dest.X + sliceSize,
            dest.Y + sliceSize,
            dividerX - (dest.X + sliceSize),
            dest.Height - sliceSize * 2
        );

        Rectangle rightArea = new(
            dividerX + dividerWidth,
            dest.Y + sliceSize,
            dest.Right - sliceSize - (dividerX + dividerWidth),
            dest.Height - sliceSize * 2
        );

        return (leftArea, rightArea);
    }

    // controle dos itens pelo mouse
    private Texture2D texture;
    private Rectangle position;
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

        if (_selectedBagIndex < 0) return;
        List<ItemStack> items = gameState.Inventory.Itens;
        if (items.Count < _selectedBagIndex) return;
        if (_itemNaMao != null) return;

        _itemNaMao ??= items[_selectedBagIndex];
        if (_itemNaMao == null) return;
        _slotOrigem = _selectedBagIndex;

        items[_selectedBagIndex] = null;
        var result = TryGetItemTexture(_itemNaMao?.Id);

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
            List<ItemStack> items = gameState.Inventory.Itens;
            items[(int)_slotOrigem] = _itemNaMao;
            texture = null;
            _selectedBagIndex = -1;
            _itemNaMao = null;
            _slotOrigem = null;
        }
    }

    private void MouseSetItem()
    {
        if (Context.Input.IsLeftClickPressed() && _slotOrigem != null)
        {
            List<ItemStack> items = gameState.Inventory.Itens;
            ItemStack existente = items[_selectedBagIndex];

            if (existente != null)
                items[(int)_slotOrigem] = existente;


            items[_selectedBagIndex] = _itemNaMao;

            texture = null;
            _selectedBagIndex = -1;
            _itemNaMao = null;
            _slotOrigem = null;
        }
    }
}
