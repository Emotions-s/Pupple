using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pupple.Objects;

public static class CardHelper
{
    public static readonly SortedDictionary<CardType, Rectangle> cardViewport = new() {
        {CardType.GreenSpecial, new Rectangle(0, 0, 240, 420)},
        {CardType.OrangeSpecial, new Rectangle(300, 0, 240, 420)},
        {CardType.PinkSpecial, new Rectangle(600, 0, 240, 420)},
        {CardType.RedSpecial, new Rectangle(900, 0, 240, 420)},
        {CardType.YellowSpecial, new Rectangle(0, 480, 240, 420)},
        {CardType.WhiteSpecial, new Rectangle(300, 480, 240, 420)},
        {CardType.BlueSpecial, new Rectangle(600, 480, 240, 420)},
        {CardType.PurpleSpecial, new Rectangle(900, 480, 240, 420)},
        {CardType.Bomb, new Rectangle(0, 960, 240, 420)},
        {CardType.Freeze, new Rectangle(300, 960, 240, 420)},
        {CardType.Rainbow, new Rectangle(600, 960, 240, 420)},
        {CardType.Shield, new Rectangle(900, 960, 240, 420)},
        {CardType.BallQueue, new Rectangle(0, 1440, 240, 420)},
        {CardType.Aim, new Rectangle(300, 1440, 240, 420)},
        {CardType.MoveDistance, new Rectangle(600, 1440, 240, 420)},
        {CardType.Null, new Rectangle(900, 1440, 240, 420)},
    };

    public static readonly Vector2 CardSize = new(240, 420);

    public static readonly SortedDictionary<BubbleColor, CardType> ColorCardMap = new() {
        {BubbleColor.Green, CardType.GreenSpecial},
        {BubbleColor.Orange, CardType.OrangeSpecial},
        {BubbleColor.Pink, CardType.PinkSpecial},
        {BubbleColor.Red, CardType.RedSpecial},
        {BubbleColor.Yellow, CardType.YellowSpecial},
        {BubbleColor.White, CardType.WhiteSpecial},
        {BubbleColor.Blue, CardType.BlueSpecial},
        {BubbleColor.Purple, CardType.PurpleSpecial},
    };
}