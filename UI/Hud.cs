using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;
using thegame.Scenes;

namespace thegame.UI;

public class Hud(GameContext context) : BaseHud(context)
{
    private readonly GameContext _context = context;
    private readonly Texture2D _layoutUiTexture = context.Content.Load<Texture2D>("UI/Hud/9-slice/Ancient/brown");
    protected InputManager inputManager = context.Input;

    private Rectangle bag;
    private Rectangle _botaoSalvarSair;

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Context.UIState.MenuAberto)
            DrawLayoutMenu(spriteBatch);


        // _hudBau.Draw(spriteBatch);
        // _hudBar.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        if (inputManager.IsKeyPressed(Keys.I))
        {
            gameState.LayoutBag = !gameState.LayoutBag;
            Context.UIState.SetMenuAberto(false);


        }

        if (inputManager.IsKeyPressed(Keys.Escape))
        {
            gameState.LayoutBag = false;
            Context.UIState.SetMenuAberto(!Context.UIState.MenuAberto);
        }

        if (Context.UIState.MenuAberto && _context.Input.WasClicked(_botaoSalvarSair))
        {
            _context.State.SaveGame();
            _context.SceneManager.ChangeScene(new MainMenuScene(_context));
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

}
