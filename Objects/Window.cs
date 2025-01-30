using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;

public class Window
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2 OriginPos { get; private set; }
    public IComponent[] Components { get; set; }
    public Color BgColor { get; private set; }
    private RenderTarget2D _target;
    public Window(int width, int height, Vector2 originPos, Color bgColor)
    {
        Width = width;
        Height = height;
        OriginPos = originPos;
        BgColor = bgColor;
        _target = new(Globals.GraphicsDevice, Width, Height);
    }

    public void Update()
    {
        if (Components == null) return;
        for (int i = 0; i < Components.Length; i++)
        {
            Components[i].Update();
        }
    }

    public RenderTarget2D GetFrame()
    {
        Globals.GraphicsDevice.SetRenderTarget(_target);
        Globals.GraphicsDevice.Clear(BgColor);

        if (Components != null)
        {
            Globals.SpriteBatch.Begin();
            foreach (var component in Components)
            {
                component.Draw();
            }
            Globals.SpriteBatch.End();
        }
        Globals.GraphicsDevice.SetRenderTarget(null);
        return _target;
    }

    public bool IsPosOver(Vector2 position)
    {
        return position.X >= OriginPos.X && position.X <= OriginPos.X + Width &&
               position.Y >= OriginPos.Y && position.Y <= OriginPos.Y + Height;
    }
}