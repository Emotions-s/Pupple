using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;

public class BombBubble : Bubble
{
    public static readonly int BombRadius = 2;
    public BombBubble(Vector2 position, Texture2D texture2D) : base(position, texture2D)
    {
        Viewport = BubbleHelper.SpecialBubbleViewPort[BubbleSpecial.Bomb];
    }
}