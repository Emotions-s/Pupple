using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;

namespace Pupple.Objects;

public static class BubbleHelper
{
    public static readonly List<BubbleColor> BubbleColors = new List<BubbleColor>
    {
        BubbleColor.Green,
        BubbleColor.Pink,
        BubbleColor.Orange,
        BubbleColor.Yellow,
        BubbleColor.Blue,
        BubbleColor.White,
        BubbleColor.Red,
        BubbleColor.Purple,
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
        {BubbleSpecial.Bomb, new Rectangle(960, 240, 60, 60)},
        {BubbleSpecial.Freeze, new Rectangle(1080, 240, 60, 60)},
        {BubbleSpecial.Rainbow, new Rectangle(1200, 240, 60, 60)},
        {BubbleSpecial.MagicGreen, new Rectangle(0, 240, 60, 60)},
        {BubbleSpecial.MagicOrange, new Rectangle(120, 240, 60, 60)},
        {BubbleSpecial.MagicPink, new Rectangle(240, 240, 60, 60)},
        {BubbleSpecial.MagicRed, new Rectangle(360, 240, 60, 60)},
        {BubbleSpecial.MagicYellow, new Rectangle(480, 240, 60, 60)},
        {BubbleSpecial.MagicWhite, new Rectangle(600, 240, 60, 60)},
        {BubbleSpecial.MagicBlue, new Rectangle(720, 240, 60, 60)},
        {BubbleSpecial.MagicPurple, new Rectangle(840, 240, 60, 60)},
    };

    public static readonly SortedDictionary<BubbleColor, BubbleSpecial> SpecialColorMap = new SortedDictionary<BubbleColor, BubbleSpecial>
    {
        {BubbleColor.Green, BubbleSpecial.MagicGreen},
        {BubbleColor.Orange, BubbleSpecial.MagicOrange},
        {BubbleColor.Pink, BubbleSpecial.MagicPink},
        {BubbleColor.Red, BubbleSpecial.MagicRed},
        {BubbleColor.Yellow, BubbleSpecial.MagicYellow},
        {BubbleColor.White, BubbleSpecial.MagicWhite},
        {BubbleColor.Blue, BubbleSpecial.MagicBlue},
        {BubbleColor.Purple, BubbleSpecial.MagicPurple},
    };


    public static readonly SortedDictionary<BubbleColor, Rectangle> SwapNormalBubbleViewPort = new SortedDictionary<BubbleColor, Rectangle>
    {
        {BubbleColor.Green, new Rectangle(0, 120, 60, 60)},
        {BubbleColor.Orange, new Rectangle(120, 120, 60, 60)},
        {BubbleColor.Pink, new Rectangle(240, 120, 60, 60)},
        {BubbleColor.Red, new Rectangle(360, 120, 60, 60)},
        {BubbleColor.Yellow, new Rectangle(480, 120, 60, 60)},
        {BubbleColor.White, new Rectangle(600, 120, 60, 60)},
        {BubbleColor.Blue, new Rectangle(720, 120, 60, 60)},
        {BubbleColor.Purple, new Rectangle(840, 120, 60, 60)},
    };

    public static readonly Rectangle LockSwapViewPort = new Rectangle(960, 120, 60, 60);

    public static readonly Rectangle ActiveShieldViewPort = new Rectangle(636, 360, 60, 60);
    public static readonly Rectangle InactiveShieldViewPort = new Rectangle(756, 360, 60, 60);
    public static readonly Rectangle ActiveMissViewPort = new Rectangle(876, 360, 60, 60);
    public static readonly Rectangle InactiveMissViewPort = new Rectangle(996, 360, 60, 60);

}