using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.UI;

public class HudManager(GameContext context) : BaseHud(context)
{
    private readonly List<BaseHud> Huds = [
            new Hud(context),
            new HudBar(context),
            new HudBau(context),
            new HudCaminhaoVenda(context),
            new HudLoja(context)
        ];


    public override void Update(GameTime gameTime)
    {
        foreach (BaseHud item in Huds)
        {
            item.Update(gameTime);
        }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (BaseHud item in Huds)
        {
            item.Draw(spriteBatch);
        }
    }
}