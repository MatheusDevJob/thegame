using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Maps;

public interface IMap
{
    int PixelWidth { get; }
    int PixelHeight { get; }

    void OnEnter();
    void OnExit();
    void Update(GameTime gameTime, TileCursor tileCursor);
    void Draw(SpriteBatch spriteBatch);
    void DrawDebug(SpriteBatch spriteBatch);
    bool Collides(Rectangle hitbox);
}