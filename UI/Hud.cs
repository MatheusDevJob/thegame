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

public class Hud
{
    private readonly GameContext _context;
    private readonly Texture2D _pixel;
    private readonly Texture2D _hudBoxItem;
    // private readonly Texture2D _hudAxeItem;
    private readonly int _scale = 3;

    public readonly List<int> boxes = [20, 74, 128, 182, 236, 290, 344];
    public int screenWidth;
    public int screenHeight;

    protected InputManager inputManager;
    private readonly Texture2D _layoutUiTexture;
    private readonly Texture2D _slotTexture;
    private readonly SpriteFont _font;
    private readonly GameState gameState;
    private Rectangle bag;
    private Rectangle _botaoSair;
    private int _selectedBagIndex = -1;
    private int _selectedHotbarIndex = -1;
    public Hud(GameContext context)
    {
        _context = context;
        _pixel = new Texture2D(_context.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _font = context.Content.Load<SpriteFont>("Fonts/MenuFont");

        _hudBoxItem = context.Content.Load<Texture2D>("UI/Hud/itemdisc_01");
        // _hudAxeItem = context.Content.Load<Texture2D>("Items/axe");
        _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");
        _slotTexture = context.Content.Load<Texture2D>("UI/Hud/itemdisc_01");
        inputManager = context.Input;
        gameState = _context.State;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;
        screenWidth = viewport.Width;
        screenHeight = viewport.Height;

        Rectangle box = new(20, 20, 220, 70);
        Rectangle lifeBackground = new(35, 45, 180, 14);
        Rectangle lifeBar = new(35, 45, (int)gameState.Player.Life, 14);
        Rectangle moneyBox = new(screenWidth - 220, 20, 200, 50);

        spriteBatch.Draw(_pixel, box, new Color(0, 0, 0, 180));
        spriteBatch.Draw(_pixel, lifeBackground, new Color(80, 80, 80, 220));
        spriteBatch.Draw(_pixel, lifeBar, new Color(180, 40, 40, 255));
        spriteBatch.Draw(_pixel, moneyBox, new Color(0, 0, 0));

        DrawItensBar(spriteBatch);
        DrawItensOnBar(spriteBatch);

        if (gameState.LayoutMenu) DrawLayoutMenu(spriteBatch);
        if (gameState.LayoutBag) DrawLayoutBag(spriteBatch);
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

    public void Update(GameTime gameTime)
    {
        // if (inputManager.IsKeyPressed(Keys.I))
        // {
        //     gameState.LayoutBag = !gameState.LayoutBag;
        //     gameState.LayoutMenu = false;
        // }

        // if (inputManager.IsKeyPressed(Keys.Escape))
        // {
        //     gameState.LayoutBag = false;
        //     gameState.LayoutMenu = !gameState.LayoutMenu;
        // }

        // if (_context.Input.WasClicked(_botaoSair))
        // {
        //     _context.State.SaveGame();
        //     _context.Game.Exit();
        // }

        EnsureHotbarSlots();

        if (inputManager.IsKeyPressed(Keys.I))
        {
            gameState.LayoutBag = !gameState.LayoutBag;
            gameState.LayoutMenu = false;
            if (!gameState.LayoutBag) _selectedBagIndex = -1;
        }

        if (inputManager.IsKeyPressed(Keys.Escape))
        {
            gameState.LayoutBag = false;
            gameState.LayoutMenu = !gameState.LayoutMenu;
            _selectedBagIndex = -1;
        }

        UpdateBagClick();
        UpdateHotbarClick();
        UpdateHotbarKeybinds();

        if (gameState.LayoutBag && _selectedHotbarIndex >= 0 && (inputManager.IsKeyPressed(Keys.Delete) || inputManager.IsKeyPressed(Keys.Back)))
            ClearHotbarSlot(_selectedHotbarIndex);

        if (_context.Input.WasClicked(_botaoSair))
        {
            _context.State.SaveGame();
            _context.Game.Exit();
        }
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

            EquipHotbarSlot(i);
            return;
        }
    }

    private void UpdateHotbarKeybinds()
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

    private void DrawItensBar(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < boxes.Count; i++)
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

        for (int i = 0; i < boxes.Count; i++)
        {
            string itemId = GetHotbarSlot(i);

            if (string.IsNullOrWhiteSpace(itemId))
                continue;

            Texture2D texture = TryGetItemTexture(itemId);

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

    private void DrawLayoutBag(SpriteBatch spriteBatch)
    {
        bag = GetBagRectangle();
        int limit = gameState.Inventory.GetLimiteItensBag;

        DrawNineSlice(spriteBatch, _layoutUiTexture, bag, 16);
        DrawTextOutlined(spriteBatch, "Inventário", new Vector2(bag.X + 35, bag.Y + 30), Color.White, 0.8f);
        DrawTextOutlined(spriteBatch, "Clique em um item e aperte 1 a 8 para equipar na barra", new Vector2(bag.X + 35, bag.Y + 58), Color.White, 0.45f);
        DrawBagSlots(spriteBatch, bag, 8, limit);
    }

    private void DrawBagSlots(SpriteBatch spriteBatch, Rectangle bag, int columns, int itens)
    {
        List<ItemStack> items = gameState.Inventory.Itens;

        for (int i = 0; i < itens; i++)
        {
            Rectangle slot = GetBagSlotRectangle(bag, columns, itens, i);
            spriteBatch.Draw(_slotTexture, slot, Color.White);

            if (i == _selectedBagIndex)
                DrawBorder(spriteBatch, slot, new Color(255, 230, 120), 2);

            if (items.Count <= i)
                continue;

            ItemStack item = items[i];

            if (item == null)
                continue;

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

    private void DrawLayoutMenu(SpriteBatch spriteBatch)
    {
        int tamanhoX = 300;
        int tamanhoY = 450;
        int width = screenWidth / 2 - tamanhoX / 2;
        int height = screenHeight / 2 - tamanhoY / 2;

        bag = new Rectangle(width, height, tamanhoX, tamanhoY);
        DrawNineSlice(spriteBatch, _layoutUiTexture, bag, 16);

        _botaoSair = new Rectangle(
            bag.X + tamanhoX / 4,
            bag.Y + tamanhoY - 65,
            150,
            30
        );

        _context.UI.DrawButton(spriteBatch, _botaoSair, "Sair");
    }

    private Rectangle GetBagRectangle()
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;
        return new Rectangle(
            150,
            100,
            viewport.Width - 300,
            viewport.Height - 200
        );
    }

    private static Rectangle GetBagSlotRectangle(Rectangle bagRect, int columns, int itens, int index)
    {
        int slotSize = 64;
        int gap = 2;
        int rows = Math.Max(1, (int)Math.Ceiling(itens / (double)columns));
        int totalWidth = columns * slotSize + (columns - 1) * gap;
        int totalHeight = rows * slotSize + (rows - 1) * gap;
        int startX = bagRect.X + (bagRect.Width - totalWidth) / 2;
        int startY = bagRect.Y + (bagRect.Height - totalHeight) / 2 + 30;
        int row = index / columns;
        int col = index % columns;

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
        int minX = boxes.Min();
        int maxX = boxes.Max() + itemWidth;
        int totalWidth = maxX - minX;
        int startX = (viewport.Width - totalWidth) / 2;
        int y = viewport.Height - 80;
        int x = startX + (boxes[index] - minX);

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
}