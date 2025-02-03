using Pupple.Objects;

namespace Pupple.States;

public class PlayerState
{
    public const int MaxBubbleQueueSize = 4;
    public const int MaxSpecialBubbleNum = 4;
    public const int MaxShooterRangeLv = 4;
    public const int MaxAimRangeLv = 4;
    public int CurrentBubbleQueueSize;
    public int ShooterRangeLv;
    public float AimRangeLv;
    public bool HaveShields;

    public bool AlreadyShieldedInRun;

    public int BombNum;

    public int FreezeNum;

    public int RainbowNum;

    public MagicBubble[] MagicBubbles;

    public PlayerState()
    {
        Reset();
    }

    public void Reset()
    {
        MagicBubbles = new MagicBubble[MaxSpecialBubbleNum];
        AlreadyShieldedInRun = false;
        CurrentBubbleQueueSize = 4;
        ShooterRangeLv = 4;
        AimRangeLv = 4;
        HaveShields = true;

        BombNum = 4;
        FreezeNum = 3;
        RainbowNum = 4;
    }
}