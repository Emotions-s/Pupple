using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pupple.Objects;

public static class BubbleHelper
{
    public static readonly List<BubbleColor> BubbleColors = new List<BubbleColor>
    {
        BubbleColor.Green,
        BubbleColor.White,
        BubbleColor.Pink,
        BubbleColor.Red,
        BubbleColor.Yellow,
        BubbleColor.Blue,
        BubbleColor.Purple,
        BubbleColor.Orange
    };

    public static readonly List<BubbleColor> BubbleColorsLv1 = new List<BubbleColor>
    {
        BubbleColor.Pink,
        BubbleColor.Yellow,
        BubbleColor.Green,
        BubbleColor.Blue,
    };

    public static readonly List<BubbleColor> BubbleColorsLv2 = new List<BubbleColor>
    {
        BubbleColor.Green,
        BubbleColor.White,
        BubbleColor.Pink,
        BubbleColor.Red,
        BubbleColor.Yellow,
    };

    public static int[][] LongRowOffsets =
        [
                [-1,  0],
                [-1, -1],
                [0, -1],
                [0,  1],
                [1,  0],
                [1, -1],
        ];

    public static int[][] ShortRowOffsets =
    [
            [-1,  0],
            [-1,  1],
            [0, -1],
            [0,  1],
            [1,  0],
            [1,  1],
    ];

    public static readonly SortedDictionary<BubbleColor, Rectangle> NormalBubbleViewPort = new SortedDictionary<BubbleColor, Rectangle>
    {
        {BubbleColor.Green, new Rectangle(0, 0, 60, 60)},
        {BubbleColor.Orange, new Rectangle(120, 0, 60, 60)},
        {BubbleColor.Pink, new Rectangle(240, 0, 60, 60)},
        {BubbleColor.Red, new Rectangle(360, 0, 60, 60)},
        {BubbleColor.Yellow, new Rectangle(480, 0, 60, 60)},
        {BubbleColor.White, new Rectangle(600, 0, 60, 60)},
        {BubbleColor.Blue, new Rectangle(720, 0, 60, 60)},
        {BubbleColor.Purple, new Rectangle(840, 0, 60, 60)},
    };

    public static readonly SortedDictionary<BubbleSpecial, Rectangle> SpecialBubbleViewPort = new SortedDictionary<BubbleSpecial, Rectangle>
    {
        {BubbleSpecial.Bomb, new Rectangle(0, 180, 60, 60)}, // red
        {BubbleSpecial.Freeze, new Rectangle(0, 360, 60, 60)}, // purple
        {BubbleSpecial.Rainbow, new Rectangle(0, 420, 60, 60)}, // orange
    };
}