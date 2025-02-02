namespace Pupple.States;

public class GameState
{
    public enum State
    {
        Playing,
        Shop,
        GameOver,
    }
    public const int MaxMissCount = 4;
    public int Level;
    public int IgnorePercent;
    public State CurrentState;
    public int CurrentLineStart;
    public int MissCount;

    public float FreezeTime;

    public GameState()
    {
        Reset();
    }

    public void Reset()
    {
        Level = 1;
        IgnorePercent = 50;
        CurrentState = State.Playing;
        CurrentLineStart = 15;
        MissCount = 0;
    }
}