using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;

namespace thegame.UI;

public class Hud
{
    private readonly GameContext _context;
    private readonly Texture2D _layoutUiTexture;
    private readonly HudBar _hudBar;
    private readonly GameState gameState;
    protected InputManager inputManager;

    private Rectangle bag;
    private Rectangle _botaoSalvarSair;

    public int screenWidth;
    public int screenHeight;

    public Hud(GameContext context)
    {
        _context = context;
        _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");
        _hudBar = new HudBar(context);
        inputManager = context.Input;
        gameState = _context.State;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Viewport viewport = _context.GraphicsDevice.Viewport;
        screenWidth = viewport.Width;
        screenHeight = viewport.Height;


        if (gameState.LayoutMenu)
            DrawLayoutMenu(spriteBatch);

        if (gameState.LayoutBag)
            _hudBar.DrawBag(spriteBatch);

        _hudBar.Draw(spriteBatch);
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

        _hudBar.Update(gameTime);

        if (gameState.LayoutMenu && _context.Input.WasClicked(_botaoSalvarSair))
        {
            _context.State.SaveGame();
            _context.Game.Exit();
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

        int widthBotao = 230;
        int heightBotao = 30;
        int meioBotoes = bag.X + tamanhoX / 2 - widthBotao / 2;

        _botaoSalvarSair = new Rectangle(
            meioBotoes,
            bag.Bottom - 105,
            widthBotao,
            heightBotao
        );

        _context.UI.DrawButton(spriteBatch, _botaoSalvarSair, "Salvar e Sair");
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
}
