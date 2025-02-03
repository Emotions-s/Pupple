using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Objects;
using Pupple.Objects.Scenes;
using Pupple.States;

namespace Pupple.Managers;

public class CardManager
{

    private readonly Dictionary<CardType, int> _cardsPercent = new()
    {
        { CardType.BallQueue, 15 },
        { CardType.Aim, 15 },
        { CardType.MoveDistance, 15 },
        { CardType.Shield, 2 },
        { CardType.Rainbow, 6 },
        { CardType.Bomb, 6 },
        { CardType.Freeze, 6 },
        { CardType.SpecialColor, 35 },
    };

    private const int NumCard = 3;
    private CardType[] _cardChoices;

    private int _activeIndex;

    private readonly Vector2[] _cardPositions = new Vector2[3]
    {
        new Vector2(Globals.ScreenW / 4, Globals.ScreenH / 2),
        new Vector2(Globals.ScreenW / 2, Globals.ScreenH / 2),
        new Vector2(Globals.ScreenW / 4  * 3, Globals.ScreenH / 2),
    };

    public CardManager()
    {

    }

    public void Update()
    {
        HandleCardSelection();
        // Console.WriteLine(_cardChoices);
        // Globals.GameState.CurrentState = GameState.State.Playing;
    }

    public void Draw()
    {
        // draw bg for shop
        Globals.SpriteBatch.Draw(Globals.Pixel, new Rectangle(0, 0, Globals.ScreenW, Globals.ScreenH), Color.Black * 0.8f);

        for (int i = 0; i < NumCard; i++)
        {
            var vp = CardHelper.cardViewport[_cardChoices[i]];
            Globals.SpriteBatch.Draw(Globals.CardSheet,
                _cardPositions[i],
                vp,
                Color.White * (_activeIndex == i ? 1f : 0.8f),
                0f,
                new Vector2(vp.Width / 2, vp.Height / 2),
                _activeIndex == i ? 1.2f : 1f,
                SpriteEffects.None,
                0f
            );
        }
    }

    private void HandleCardSelection()
    {
        // CardType
        if (InputManager.MousePosition.Y >= (Globals.ScreenH - CardHelper.CardSize.Y) / 2
            && InputManager.MousePosition.Y <= (Globals.ScreenH + CardHelper.CardSize.Y / 2))
        {
            if (InputManager.MousePosition.X >= _cardPositions[0].X - CardHelper.CardSize.X / 2
                && InputManager.MousePosition.X <= _cardPositions[0].X + CardHelper.CardSize.X / 2)
            {
                _activeIndex = 0;
            }
            else if (InputManager.MousePosition.X >= _cardPositions[1].X - CardHelper.CardSize.X / 2
                && InputManager.MousePosition.X <= _cardPositions[1].X + CardHelper.CardSize.X / 2)
            {
                _activeIndex = 1;
            }
            else if (InputManager.MousePosition.X >= _cardPositions[2].X - CardHelper.CardSize.X / 2
                && InputManager.MousePosition.X <= _cardPositions[2].X + CardHelper.CardSize.X / 2)
            {
                _activeIndex = 2;
            }
            else
            {
                _activeIndex = -1;
            }
        }
        else
        {
            _activeIndex = -1;
        }

        if (_activeIndex != -1 && InputManager.Clicked)
        {
            SelectCardType(_cardChoices[_activeIndex]);
            PlayScene.ChangeGameState(GameState.State.Playing);
        }
    }

    private void SelectCardType(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.BallQueue:
                Globals.PlayerState.CurrentBubbleQueueSize++;
                break;
            case CardType.Aim:
                Globals.PlayerState.AimRangeLv++;
                break;
            case CardType.MoveDistance:
                Globals.PlayerState.ShooterRangeLv++;
                break;
            case CardType.Shield:
                Globals.PlayerState.HaveShields = true;
                Globals.PlayerState.AlreadyShieldedInRun = true;
                break;
            case CardType.Rainbow:
                Globals.PlayerState.RainbowNum++;
                break;
            case CardType.Bomb:
                Globals.PlayerState.BombNum++;
                break;
            case CardType.Freeze:
                Globals.PlayerState.FreezeNum++;
                break;
            case CardType.GreenSpecial:
                AddMagicBubble(BubbleColor.Green);
                break;
            case CardType.OrangeSpecial:
                AddMagicBubble(BubbleColor.Orange);
                break;
            case CardType.PinkSpecial:
                AddMagicBubble(BubbleColor.Pink);
                break;
            case CardType.RedSpecial:
                AddMagicBubble(BubbleColor.Red);
                break;
            case CardType.YellowSpecial:
                AddMagicBubble(BubbleColor.Yellow);
                break;
            case CardType.WhiteSpecial:
                AddMagicBubble(BubbleColor.White);
                break;
            case CardType.BlueSpecial:
                AddMagicBubble(BubbleColor.Blue);
                break;
            case CardType.PurpleSpecial:
                AddMagicBubble(BubbleColor.Purple);
                break;
        }
        Globals.PickCardSoundInstance.Play();
    }

    public void DrawCards()
    {
        _cardChoices = new CardType[NumCard];
        List<CardType> possibleSpecialColor = new();
        Globals.GameState.BubbleColorsInGame.ForEach(color =>
        {
            possibleSpecialColor.Add(CardHelper.ColorCardMap[color]);
        });
        var possibleCards = getPossibleCard();
        for (int i = 0; i < NumCard; i++)
        {
            _cardChoices[i] = RandomCard(possibleCards, possibleSpecialColor);
        }
    }

    private CardType RandomCard(List<CardType> possibleCards, List<CardType> possibleSpecialColor)
    {
        CardType cardType = CardType.Null;

        if (possibleCards.Count == 0)
        {
            return CardType.Null;
        }

        while (cardType == CardType.Null)
        {
            int n = Globals.Instance.Random.Next(1, 100);
            foreach (var card in _cardsPercent)
            {
                n -= card.Value;
                if (n <= 0)
                {
                    if (possibleCards.Contains(card.Key))
                    {
                        if (card.Key == CardType.SpecialColor)
                        {
                            cardType = RandomSpecialColor(possibleSpecialColor);
                            possibleSpecialColor.Remove(cardType);
                            break;
                        }
                        cardType = card.Key;
                        possibleCards.Remove(cardType);
                        break;
                    }
                }
            }
        }
        return cardType;
    }
    private List<CardType> getPossibleCard()
    {
        List<CardType> list = new();

        if (Globals.PlayerState.CurrentBubbleQueueSize < PlayerState.MaxBubbleQueueSize)
        {
            list.Add(CardType.BallQueue);
        }
        if (Globals.PlayerState.AimRangeLv < PlayerState.MaxAimRangeLv)
        {
            list.Add(CardType.Aim);
        }
        if (Globals.PlayerState.ShooterRangeLv < PlayerState.MaxShooterRangeLv)
        {
            list.Add(CardType.MoveDistance);
        }
        if (!Globals.PlayerState.HaveShields && !Globals.PlayerState.AlreadyShieldedInRun)
        {
            list.Add(CardType.Shield);
        }
        if (Globals.PlayerState.RainbowNum < PlayerState.MaxSpecialBubbleNum)
        {
            list.Add(CardType.Rainbow);
        }
        if (Globals.PlayerState.BombNum < PlayerState.MaxSpecialBubbleNum)
        {
            list.Add(CardType.Bomb);
        }
        if (Globals.PlayerState.FreezeNum < PlayerState.MaxSpecialBubbleNum)
        {
            list.Add(CardType.Freeze);
        }
        if (Globals.PlayerState.MagicBubbles[PlayerState.MaxSpecialBubbleNum - 1] == null)
        {
            list.Add(CardType.SpecialColor);
        }
        return list;
    }
    private CardType RandomSpecialColor(List<CardType> possibleColorCards)
    {
        return possibleColorCards[Globals.Instance.Random.Next(0, possibleColorCards.Count)];
    }

    private void AddMagicBubble(BubbleColor color)
    {
        for (int i = 0; i < PlayerState.MaxSpecialBubbleNum; i++)
        {
            if (Globals.PlayerState.MagicBubbles[i] == null)
            {
                Globals.PlayerState.MagicBubbles[i] = new MagicBubble(Vector2.Zero, Globals.ShooterSceneSheet, color);
                break;
            }
        }
    }
}