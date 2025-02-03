using System;
using System.Collections.Generic;
using Pupple.Objects;

namespace Pupple.States;

public class GameState
{
    public enum State
    {
        Playing,
        Shop,
        GameOver,
    }

    public const int AddColorRound = 10;
    public const int AddStartLineRound = 3;
    public const int AddMaxMissCountRound = 12;


    public List<BubbleColor> BubbleColorsInGame;
    private const int MinMissCount = 2;
    private const int MinIgnorePercent = 20;
    private const int MaxStartLine = 10;

    private const int StartColor = 4;

    public int Level;

    public int MaxMissCount;
    public int MissCount;
    public int IgnorePercent;
    public int StartLine;
    public int AmountTotalLine;
    public int CurrentAmountTotalLine;
    public float FreezeTime;
    public State CurrentState;

    public GameState()
    {
        Reset();
    }

    public void Reset()
    {
        CurrentState = State.Playing;
        Level = 1;
        BubbleColorsInGame = new();
        for (int i = 0; i < StartColor; i++)
        {
            BubbleColorsInGame.Add((BubbleColor)i);
        }

        MaxMissCount = 4;
        MissCount = 0;
        IgnorePercent = 50;
        StartLine = 4;
        AmountTotalLine = 10;
        CurrentAmountTotalLine = 0;
    }

    public void LevelUp()
    {
       Globals.WinSoundInstance.Play();
       Level++;

        // add new color every 10 levels
        if (Level % AddColorRound == 0)
        {
            var n = Math.Min(Level / AddColorRound + StartColor - 1, BubbleHelper.BubbleColors.Count - 1);
            BubbleColorsInGame.Add((BubbleColor)n);
        }

        // increase start line every 3 levels
        if (Level % AddStartLineRound == 0)
        {
            StartLine = Math.Min(StartLine + 1, MaxStartLine);
        }

        // increase total line every level
        AmountTotalLine++;

        // increase ignore percent every level
        IgnorePercent = Math.Max(MinIgnorePercent, IgnorePercent - 2);

        // increase miss count every 12 levels
        if (Level % AddMaxMissCountRound == 0)
        {
            MaxMissCount = Math.Max(MaxMissCount - 1, MinMissCount);
        }

        // reset everything
        MissCount = 0;
        CurrentAmountTotalLine = AmountTotalLine;
        FreezeTime = 0;
    }
}