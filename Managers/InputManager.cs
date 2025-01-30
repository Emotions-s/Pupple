using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pupple.Managers;

public static class InputManager
{
    // Keyboard
    private static KeyboardState _prevKeyboard;
    private static KeyboardState _currentKeyboard;

    // Mouse
    private static MouseState _prevMouse;
    private static MouseState _currentMouse;

    // Mouse properties
    public static bool Clicked { get; private set; }
    public static bool Released { get; private set; }
    public static bool MousePressed { get; private set; }
    public static bool RightClicked { get; private set; }
    public static Vector2 MousePosition => _currentMouse.Position.ToVector2();

    public static bool IsLeftHeld => KeyDown(Keys.Left) || KeyDown(Keys.A);
    public static bool IsRightHeld => KeyDown(Keys.Right) || KeyDown(Keys.D);

    public static bool KeyPressed(Keys key)
    {
        return _currentKeyboard.IsKeyDown(key) && _prevKeyboard.IsKeyUp(key);
    }

    public static bool KeyReleased(Keys key)
    {
        return _currentKeyboard.IsKeyUp(key) && _prevKeyboard.IsKeyDown(key);
    }

    public static bool KeyDown(Keys key)
    {
        return _currentKeyboard.IsKeyDown(key);
    }

    public static void Update()
    {
        // Store previous states
        _prevKeyboard = _currentKeyboard;
        _prevMouse = _currentMouse;

        // Read new states
        _currentKeyboard = Keyboard.GetState();
        _currentMouse = Mouse.GetState();

        // Determine mouse button transitions
        Clicked = _currentMouse.LeftButton == ButtonState.Pressed
                  && _prevMouse.LeftButton == ButtonState.Released;
        Released = _currentMouse.LeftButton == ButtonState.Released
                   && _prevMouse.LeftButton == ButtonState.Pressed;
        RightClicked = _currentMouse.RightButton == ButtonState.Pressed
                       && _prevMouse.RightButton == ButtonState.Released;
        MousePressed = _currentMouse.LeftButton == ButtonState.Pressed;
    }

}