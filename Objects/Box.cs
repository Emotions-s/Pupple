using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;
public abstract class Box : IComponent
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2 OriginPos { get; private set; }
    public string Header { get; private set; }
    public Color BgColor { get; private set; }

    public Color FontColor { get; set; }

    public Box(int width, int height, Vector2 originPos, string header, Color bgColor, Color fontColor)
    {
        Width = width;
        Height = height;
        OriginPos = originPos;
        Header = header;
        BgColor = bgColor;
        FontColor = fontColor;
    }

    public virtual void Reset()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Draw()
    {
    }

}