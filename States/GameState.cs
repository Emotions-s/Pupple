namespace Pupple.States;

public class GameState
{
    public const int MaxMissCount = 4;
    public int Level;
    public int IgnorePercent;
    public bool IsDead;
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
        IsDead = false;
        CurrentLineStart = 2;
        MissCount = 0;
    }
}