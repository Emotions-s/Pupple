namespace Pupple.States;

public class PlayerState
{
    public const int BubbleQueueMaxSize = 4;
    public int CurrentBubbleQueueSize;
    public int ShooterRangeLv;
    public float AimRangeLv;
    public bool HaveShields;

    public PlayerState()
    {
        Reset();
    }

    public void Reset()
    {
        CurrentBubbleQueueSize = 3;
        ShooterRangeLv = 0;
        AimRangeLv = 0;
        HaveShields = false;
    }
}