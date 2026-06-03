using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace thegame.Core;

public class InputManager
{
    private KeyboardState _currentKeyboard;
    private KeyboardState _previousKeyboard;
    private MouseState _currentMouse;
    private MouseState _previousMouse;
    public virtual Point Position => Mouse.GetState().Position;

    public void Update()
    {
        _previousKeyboard = _currentKeyboard;
        _previousMouse = _currentMouse;

        _currentKeyboard = Keyboard.GetState();
        _currentMouse = Mouse.GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return _currentKeyboard.IsKeyDown(key);
    }

    public bool IsKeyPressed(Keys key)
    {
        return _currentKeyboard.IsKeyDown(key) && _previousKeyboard.IsKeyUp(key);
    }

    public bool IsKeyReleased(Keys key)
    {
        return _currentKeyboard.IsKeyUp(key) && _previousKeyboard.IsKeyDown(key);
    }

    public bool IsLeftClickPressed()
    {
        return _currentMouse.LeftButton == ButtonState.Pressed &&
               _previousMouse.LeftButton == ButtonState.Released;
    }
    public bool IsRightClickPressed()
    {
        return _currentMouse.RightButton == ButtonState.Pressed &&
               _previousMouse.RightButton == ButtonState.Released;
    }


    public bool WasClicked(Rectangle rectangle)
    {
        MouseState mouse = _currentMouse;
        return rectangle.Contains(mouse.Position) &&
               mouse.LeftButton == ButtonState.Pressed &&
               _previousMouse.LeftButton == ButtonState.Released;
    }
}