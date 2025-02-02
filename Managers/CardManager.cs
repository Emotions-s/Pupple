using System.Collections.Generic;

namespace Pupple.Objects;

public class CardManager
{

    private readonly Dictionary<int, CardType> _cardsPercent = new()
    {
        {15, CardType.BallQueue},
        {15, CardType.Aim},
        {15, CardType.MoveDistance},
        {2, CardType.Shield},
        {6, CardType.Rainbow},
        {6, CardType.Bomb},
        {6, CardType.Freeze},
        {35, CardType.SpecialColor},

    };

    public CardManager()
    {
    }

    public void Update()
    {

    }

    public void Draw()
    {

    }

    private CardType RandomCard()
    {
        int n = Globals.Instance.Random.Next(1, 100);
        foreach (var percent in _cardsPercent.Keys)
        {
            n -= percent;
            if (n <= 0)
            {
                if (_cardsPercent[percent] == CardType.SpecialColor)
                {
                    // TODO: Implement SpecialColor
                    return CardType.SpecialColor;
                }
                return _cardsPercent[percent];
            }
        }
        return CardType.BallQueue;
    }
}