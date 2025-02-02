using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pupple.Objects;

public class FreezeBubble : Bubble
{
    public static readonly float FreezeTime = 5f;
    public FreezeBubble(Vector2 position, Texture2D texture2D) : base(position, texture2D)
    {
        Viewport = BubbleHelper.SpecialBubbleViewPort[BubbleSpecial.Freeze];
    }
}