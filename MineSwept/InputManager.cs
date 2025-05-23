using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MineSwept;

public enum MouseButton
{
    Left, Middle, Right
}


public static class InputManager
{
    public static KeyboardState KeyboardState { get; private set; }
    public static KeyboardState KeyboardStateOld { get; private set; }

    public static MouseState MouseState { get; private set; }
    public static MouseState MouseStateOld { get; private set; }

    public static Vector2 MovementVelocityOld = Vector2.Zero;
    public static Vector2 MovementVelocity = Vector2.Zero;
    public static Vector2 OneHitMovementVelocity = Vector2.Zero;


    private static int lastScrollWheelValue = 0;


    public static void Update()
    {

        KeyboardStateOld = KeyboardState;
        KeyboardState = Keyboard.GetState();

        MouseStateOld = MouseState;
        MouseState = Mouse.GetState();


        MovementVelocity = Vector2.Zero;
        OneHitMovementVelocity = Vector2.Zero;

        MovementVelocityOld = MovementVelocity;


        if (MovementVelocity != Vector2.Zero)
            MovementVelocity = Vector2.Normalize(MovementVelocity);

        
        if (MouseState.ScrollWheelValue > lastScrollWheelValue)
        {
            ScrollUpEvent?.Invoke(null, EventArgs.Empty);
            lastScrollWheelValue = MouseState.ScrollWheelValue;
        }
        else if (MouseState.ScrollWheelValue < lastScrollWheelValue)
        {
            ScrollDownEvent?.Invoke(null, EventArgs.Empty);
            lastScrollWheelValue = MouseState.ScrollWheelValue;
        }
    }


    public static bool Fullscreen => GetKeyDown(Keys.F11);

    public static bool GetKeyDown(Keys key) => KeyboardState.IsKeyDown(key) && KeyboardStateOld.IsKeyUp(key);
    public static bool GetKeyUp(Keys key) => KeyboardState.IsKeyUp(key) && KeyboardStateOld.IsKeyDown(key);

    public static bool GetMouseButtonDown(MouseButton button) => button switch
    {
        MouseButton.Right => MouseState.RightButton == ButtonState.Pressed && MouseStateOld.RightButton == ButtonState.Released,
        MouseButton.Middle => MouseState.MiddleButton == ButtonState.Pressed && MouseStateOld.MiddleButton == ButtonState.Released,
        _ => MouseState.LeftButton == ButtonState.Pressed && MouseStateOld.LeftButton == ButtonState.Released
    };

    public static bool GetMouseButtonUp(MouseButton button) => button switch
    {
        MouseButton.Right => MouseState.RightButton == ButtonState.Released && MouseStateOld.RightButton == ButtonState.Pressed,
        MouseButton.Middle => MouseState.MiddleButton == ButtonState.Released && MouseStateOld.MiddleButton == ButtonState.Pressed,
        _ => MouseState.LeftButton == ButtonState.Released && MouseStateOld.LeftButton == ButtonState.Pressed
    };

    public static event EventHandler ScrollUpEvent;
    public static event EventHandler ScrollDownEvent;
}