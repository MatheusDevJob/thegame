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

        spriteBatch.Draw(_pixel, box, new Color(0, 0, 0, 180));

        Rectangle lifeBackground = new(35, 45, 180, 14);
        Rectangle lifeBar = new(35, 45, (int)gameState.Player.Life, 14);

        spriteBatch.Draw(_pixel, lifeBackground, new Color(80, 80, 80, 220));
        spriteBatch.Draw(_pixel, lifeBar, new Color(180, 40, 40, 255));



        Rectangle moneyBox = new(screenWidth - 220, 20, 200, 50);
        spriteBatch.Draw(_pixel, moneyBox, new Color(0, 0, 0));

        DrawItensBar(spriteBatch);
        DrawItensOnBar(spriteBatch);
        if (gameState.LayoutMenu) DrawLayoutMenu(spriteBatch);
        if (gameState.LayoutBag) DrawLayoutBag(spriteBatch);
    }

    private void DrawLayoutBag(SpriteBatch spriteBatch)
    {
        Rectangle bag = new(
            150,
            100,
            screenWidth - 300,
            screenHeight - 200
        );
        int LimiteItensBag = gameState.Inventory.GetLimiteItensBag;

        DrawNineSlice(spriteBatch, _layoutUiTexture, bag, 16);
        DrawBagSlots(spriteBatch, bag, 8, LimiteItensBag);
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

    private void DrawBagSlots(SpriteBatch spriteBatch, Rectangle bag, int columns, int itens)
    {
        int slotSize = 64;
        int gap = 2;

        int totalWidth = columns * slotSize + (columns - 1) * gap;
        int totalHeight = itens * slotSize + (itens - 1) * gap;

        int startX = bag.X + (bag.Width - totalWidth) / 2;
        int startY = bag.Y + 90;

        List<ItemStack> items = gameState.Inventory.Itens;

        for (int i = 0; i < itens; i++)
        {
            int row = i / columns;
            int col = i % columns;

            Rectangle slot = new(
                startX + col * (slotSize + gap),
                startY + row * (slotSize + gap),
                slotSize,
                slotSize
            );
            spriteBatch.Draw(_slotTexture, slot, Color.White);

            Rectangle itemRect = new(
                slot.X + 10,
                slot.Y + 8,
                slot.Width - 20,
                slot.Height - 20
            );

            if (items.Count <= i) continue;
            ItemStack item = items[i];
            if (item == null) continue;

            Texture2D textureItem = EntityTexture2D.GetEntityTextureById(_context, item.Id);
            spriteBatch.Draw(textureItem, itemRect, Color.White);

            string quantidade = item.Quantidade.ToString();
            float textScale = 1f;
            Vector2 textSize = _font.MeasureString(quantidade) * textScale;
            Vector2 textPosition = new(
                slot.Right - textSize.X - 8,
                slot.Bottom - textSize.Y - 6
            );

            spriteBatch.DrawString(_font, quantidade, textPosition + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, quantidade, textPosition, Color.White, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);
        }
    }

    public void Update(GameTime gameTime)
    {
        if (inputManager.IsKeyPressed(Keys.I))
        {
            gameState.LayoutBag = !gameState.LayoutBag;
            gameState.LayoutMenu = false;
        }

        if (inputManager.IsKeyPressed(Keys.Escape))
        {
            gameState.LayoutBag = false;
            gameState.LayoutMenu = !gameState.LayoutMenu;
        }

        if (_context.Input.WasClicked(_botaoSair))
        {
            _context.State.SaveGame();
            _context.Game.Exit();
        }
    }

    private void DrawItensBar(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;

        int screenWidth = viewport.Width;
        int screenHeight = viewport.Height;

        int itemWidth = 18 * _scale;
        int itemHeight = 19 * _scale;

        int minX = boxes.Min();
        int maxX = boxes.Max() + itemWidth;
        int totalWidth = maxX - minX;

        int startX = (screenWidth - totalWidth) / 2;
        int y = screenHeight - 80;

        for (int i = 0; i < boxes.Count; i++)
        {
            int x = startX + (boxes[i] - minX);

            Rectangle bottomBox = new(x, y, itemWidth, itemHeight);
            spriteBatch.Draw(_hudBoxItem, bottomBox, Color.White);
        }
    }

    private void DrawItensOnBar(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;

        int screenWidth = viewport.Width;
        int screenHeight = viewport.Height;

        int itemWidth = 18 * _scale - 5;
        int itemHeight = 19 * _scale - 5;

        int minX = boxes.Min();
        int maxX = boxes.Max() + itemWidth;
        int totalWidth = maxX - minX;

        int startX = (screenWidth - totalWidth) / 2;
        int y = screenHeight - 80;

        List<string> ListTools = gameState.PlayerSave.ListTools;

        for (int i = 0; i < boxes.Count; i++)
        {
            if (i >= ListTools.Count)
                continue;

            int x = startX + (boxes[i] - minX);
            Entity tool = EntityFactory.Create(_context, new TiledObjectData { Type = ListTools[i], X = 1, Y = 1 });

            if (tool == null || tool.Sprite == null)
                continue;

            Rectangle itemRect = new(
                x + 6,
                y + 4,
                itemWidth - 12,
                itemHeight - 8
            );

            spriteBatch.Draw(tool.Sprite, itemRect, Color.White);
        }
    }

    private void DrawLayoutMenu(SpriteBatch spriteBatch)
    {
        DrawNineSlice(spriteBatch, _layoutUiTexture, bag, 16);

        int tamanhoX = 300;
        int tamanhoY = 450;

        int Width = screenWidth / 2 - tamanhoX / 2;
        int Height = screenHeight / 2 - tamanhoY / 2;

        bag = new(
           Width,
           Height,
           tamanhoX,
           tamanhoY
       );

        _botaoSair = new(
           bag.X + tamanhoX / 4,
           bag.Y + tamanhoY - 65,
           150,
           30
       );
        _context.UI.DrawButton(spriteBatch, _botaoSair, "Sair");
    }
}