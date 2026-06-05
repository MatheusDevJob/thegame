using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Entities;
using thegame.Items;
using thegame.Maps;

namespace thegame.UI;

public class HudBar
{
    private readonly GameContext _context;
    private readonly Texture2D _pixel;
    private readonly Texture2D _hudBoxItem;
    private readonly Texture2D _layoutUiTexture;
    private readonly Texture2D _slotTexture;
    private readonly SpriteFont _font;
    private readonly GameState gameState;
    private readonly InputManager inputManager;
    private readonly int _scale = 3;


    public readonly List<string> boxes = ["AxeTool", "PickaxeTool", "ShovelTool"];
    private const int HotbarSlots = 8;
    private const int SlotSize = 60;
    private const int SlotGap = 0;
    private ItemStack _itemNaMao;
    private int? _slotOrigem;

    private Rectangle bag;
    private int _selectedBagIndex = -1;
    private int _selectedHotbarIndex = 0;

    public HudBar(GameContext context)
    {
        _context = context;
        _pixel = new Texture2D(_context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _font = context.Content.Load<SpriteFont>("Fonts/MenuFont");
        _hudBoxItem = context.Content.Load<Texture2D>("UI/Hud/itemdisc_01");
        _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");
        _slotTexture = context.Content.Load<Texture2D>("UI/Hud/itemdisc_01");

        inputManager = context.Input;
        gameState = _context.State;
    }

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

        DrawNineSlice(spriteBatch, _layoutUiTexture, bag, 16);
        var (leftArea, rightArea) = DrawNineSliceSplit(spriteBatch, _layoutUiTexture, bag, 16, 0.60f);
        DrawBagSlots(spriteBatch, bag, 8, limit);
        DrawBagSlotsHorizontalDivider(spriteBatch, bag, 8, limit, 16);
    }

    public void Update(GameTime gameTime)
    {
        EnsureHotbarSlots();

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

            if (!_context.Input.WasClicked(slot))
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

            if (!_context.Input.WasClicked(slot))
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

        if (string.IsNullOrWhiteSpace(itemId))
            return;

        Entity equipable = EntityFactory.Create(_context, new TiledObjectData
        {
            Type = itemId,
            X = gameState.Player.Posicao.X,
            Y = gameState.Player.Posicao.Y
        });

        if (equipable == null)
            return;

        gameState.ActiveEquipe = equipable;
    }

    private bool CanUseAsHotbarItem(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
            return false;

        Texture2D texture = TryGetItemTexture(itemId);
        return texture != null;
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
        EnsureHotbarSlots();

        List<ItemStack> items = gameState.Inventory.Itens;
        for (int i = 0; i < 8; i++)
        {
            ItemStack item = items[i];
            if (item == null) continue;

            Texture2D texture = TryGetItemTexture(item.Id);

            if (texture == null)
                continue;

            Rectangle slot = GetHotbarSlotRectangle(i);
            Rectangle itemRect = new(
                slot.X + 8,
                slot.Y + 8,
                slot.Width - 16,
                slot.Height - 16
            );

            spriteBatch.Draw(texture, itemRect, Color.White);
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
            Texture2D textureItem = TryGetItemTexture(item.Id);

            if (textureItem == null)
                continue;

            Rectangle itemRect = new(
                slot.X + 10,
                slot.Y + 8,
                slot.Width - 20,
                slot.Height - 20
            );

            spriteBatch.Draw(textureItem, itemRect, Color.White);

            string quantidade = item.Quantidade.ToString();
            Vector2 textSize = _font.MeasureString(quantidade) * 0.8f;
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
            sliceSize
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
        Viewport viewport = _context.GraphicsDevice.Viewport;

        return new Rectangle(
            450,
            250,
            viewport.Width - 900,
            viewport.Height - 500
        );
    }

    private static Rectangle GetBagSlotRectangle(Rectangle bagRect, int columns, int itens, int index)
    {
        int slotSize = 64;
        int gap = 2;


        int startX = bagRect.X + 40;
        int startY = bagRect.Y + 30;
        int row = index / columns;
        int col = index % columns;

        if (index > 7)
        {
            startY += 20;
        }
        return new Rectangle(
            startX + col * (slotSize + gap),
            startY + row * (slotSize + gap),
            slotSize,
            slotSize
        );
    }

    private Rectangle GetHotbarSlotRectangle(int index)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;
        int itemWidth = 18 * _scale;
        int itemHeight = 19 * _scale;
        int startY = viewport.Height - itemHeight - 10;

        int x = viewport.Width - 70;
        int y = startY - (HotbarSlots - 1 - index) * (SlotSize + SlotGap);
        return new Rectangle(x, y, itemWidth, itemHeight);
    }

    private string GetHotbarSlot(int slotIndex)
    {
        EnsureHotbarSlots();

        if (slotIndex < 0 || slotIndex >= gameState.PlayerSave.EquipableIds.Count)
            return "";

        return gameState.PlayerSave.EquipableIds[slotIndex] ?? "";
    }

    private void SetHotbarSlot(int slotIndex, string itemId)
    {
        EnsureHotbarSlots();

        if (slotIndex < 0 || slotIndex >= gameState.PlayerSave.EquipableIds.Count)
            return;

        gameState.PlayerSave.EquipableIds[slotIndex] = itemId ?? "";
    }

    private void ClearHotbarSlot(int slotIndex)
    {
        SetHotbarSlot(slotIndex, "");

        if (_selectedHotbarIndex == slotIndex)
            gameState.ActiveEquipe = null;
    }

    private void EnsureHotbarSlots()
    {
        gameState.PlayerSave.EquipableIds ??= [];

        while (gameState.PlayerSave.EquipableIds.Count < 8)
            gameState.PlayerSave.EquipableIds.Add("");

        if (gameState.PlayerSave.EquipableIds.Count > 8)
            gameState.PlayerSave.EquipableIds.RemoveRange(8, gameState.PlayerSave.EquipableIds.Count - 8);
    }

    private Texture2D TryGetItemTexture(string itemId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return null;

            return EntityTexture2D.GetEntityTextureById(_context, itemId);
        }
        catch
        {
            return null;
        }
    }

    private void DrawTextOutlined(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float scale)
    {
        spriteBatch.DrawString(_font, text, position + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    private void DrawBorder(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
    {
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
    }

    private static void DrawNineSlice(SpriteBatch spriteBatch, Texture2D texture, Rectangle dest, int sliceSize)
    {
        int w = texture.Width;
        int h = texture.Height;

        Rectangle srcTopLeft = new(0, 0, sliceSize, sliceSize);
        Rectangle srcTop = new(sliceSize, 0, w - sliceSize * 2, sliceSize);
        Rectangle srcTopRight = new(w - sliceSize, 0, sliceSize, sliceSize);
        Rectangle srcLeft = new(0, sliceSize, sliceSize, h - sliceSize * 2);
        Rectangle srcCenter = new(sliceSize, sliceSize, w - sliceSize * 2, h - sliceSize * 2);
        Rectangle srcRight = new(w - sliceSize, sliceSize, sliceSize, h - sliceSize * 2);
        Rectangle srcBottomLeft = new(0, h - sliceSize, sliceSize, sliceSize);
        Rectangle srcBottom = new(sliceSize, h - sliceSize, w - sliceSize * 2, sliceSize);
        Rectangle srcBottomRight = new(w - sliceSize, h - sliceSize, sliceSize, sliceSize);

        Rectangle dstTopLeft = new(dest.X, dest.Y, sliceSize, sliceSize);
        Rectangle dstTop = new(dest.X + sliceSize, dest.Y, dest.Width - sliceSize * 2, sliceSize);
        Rectangle dstTopRight = new(dest.Right - sliceSize, dest.Y, sliceSize, sliceSize);
        Rectangle dstLeft = new(dest.X, dest.Y + sliceSize, sliceSize, dest.Height - sliceSize * 2);
        Rectangle dstCenter = new(dest.X + sliceSize, dest.Y + sliceSize, dest.Width - sliceSize * 2, dest.Height - sliceSize * 2);
        Rectangle dstRight = new(dest.Right - sliceSize, dest.Y + sliceSize, sliceSize, dest.Height - sliceSize * 2);
        Rectangle dstBottomLeft = new(dest.X, dest.Bottom - sliceSize, sliceSize, sliceSize);
        Rectangle dstBottom = new(dest.X + sliceSize, dest.Bottom - sliceSize, dest.Width - sliceSize * 2, sliceSize);
        Rectangle dstBottomRight = new(dest.Right - sliceSize, dest.Bottom - sliceSize, sliceSize, sliceSize);

        spriteBatch.Draw(texture, dstTopLeft, srcTopLeft, Color.White);
        spriteBatch.Draw(texture, dstTop, srcTop, Color.White);
        spriteBatch.Draw(texture, dstTopRight, srcTopRight, Color.White);
        spriteBatch.Draw(texture, dstLeft, srcLeft, Color.White);
        spriteBatch.Draw(texture, dstCenter, srcCenter, Color.White);
        spriteBatch.Draw(texture, dstRight, srcRight, Color.White);
        spriteBatch.Draw(texture, dstBottomLeft, srcBottomLeft, Color.White);
        spriteBatch.Draw(texture, dstBottom, srcBottom, Color.White);
        spriteBatch.Draw(texture, dstBottomRight, srcBottomRight, Color.White);
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
            spriteBatch.Draw(texture, rect, Color.White);
        }

        if (_selectedBagIndex < 0) return;
        List<ItemStack> items = gameState.Inventory.Itens;
        if (items.Count < _selectedBagIndex) return;
        if (_itemNaMao != null) return;

        _itemNaMao ??= items[_selectedBagIndex];
        if (_itemNaMao == null) return;
        _slotOrigem = _selectedBagIndex;

        items[_selectedBagIndex] = null;
        texture = TryGetItemTexture(_itemNaMao?.Id);
    }

    private void CancelarMouseSetItem()
    {
        if (_context.Input.IsRightClickPressed() && _slotOrigem != null)
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
        if (_context.Input.IsLeftClickPressed() && _slotOrigem != null)
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
