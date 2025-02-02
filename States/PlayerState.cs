namespace Pupple.States;

public class PlayerState
{
    public const int MaxBubbleQueueSize = 4;

    public const int MaxSpecialBubbleNum = 4;
    public int CurrentBubbleQueueSize;
    public int ShooterRangeLv;
    public float AimRangeLv;
    public bool HaveShields;

    public int BombNum;

    public int FreezeNum;

    public int RainbowNum;

    public PlayerState()
    {
        Reset();
    }

    public void Reset()
    {
        CurrentBubbleQueueSize = 3;
        ShooterRangeLv = 0;
        AimRangeLv = 0;
        HaveShields = true;

        BombNum = 1;
        FreezeNum = 2;
        RainbowNum = 4;
    }
}