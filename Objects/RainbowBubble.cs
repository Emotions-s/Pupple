using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;

public class RainbowBubble : Bubble
{
    public RainbowBubble(Vector2 position, Texture2D texture2D) : base(position, texture2D)
    {
        Viewport = BubbleHelper.SpecialBubbleViewPort[BubbleSpecial.Rainbow];
    }
}