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

    public static readonly SortedDictionary<BubbleColor, Rectangle> BubbleViewPort = new SortedDictionary<BubbleColor, Rectangle>
    {
        {BubbleColor.Green, new Rectangle(0, 0, 60, 60)},
        {BubbleColor.White, new Rectangle(0, 60, 60, 60)},
        {BubbleColor.Pink, new Rectangle(0, 120, 60, 60)},
        {BubbleColor.Red, new Rectangle(0, 180, 60, 60)},
        {BubbleColor.Yellow, new Rectangle(0, 240, 60, 60)},
        {BubbleColor.Blue, new Rectangle(0, 300, 60, 60)},
        {BubbleColor.Purple, new Rectangle(0, 360, 60, 60)},
        {BubbleColor.Orange, new Rectangle(0, 420, 60, 60)},
    };
}