using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;

public class NormalBubble : Bubble
{
    public BubbleColor Color { get; }

    public NormalBubble(Vector2 position, Texture2D texture2D, BubbleColor color) : base(position, texture2D)
    {
        Color = color;
        Viewport = BubbleHelper.BubbleViewPort[Color];
    }


}