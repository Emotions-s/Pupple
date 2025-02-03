using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;

public class MagicBubble : NormalBubble
{

    public MagicBubble(Vector2 position, Texture2D texture2D, BubbleColor color) : base(position, texture2D, color)
    {
        Viewport = BubbleHelper.SpecialBubbleViewPort[BubbleHelper.SpecialColorMap[color]];
    }

}