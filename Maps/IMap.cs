using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thegame.Maps;

public interface IMap
{
    int PixelWidth { get; }
    int PixelHeight { get; }

    void OnEnter();
    void OnExit();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    bool Collides(Rectangle hitbox);
}