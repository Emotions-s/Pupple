using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Managers;
using Pupple.States;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Pupple.Objects;

namespace Pupple;

class Globals
{
    public const int ScreenW = 1920;
    public const int ScreenH = 1080;
    public const int GridSize = 60;
    public const float BubbleRadius = 28f;
    public const float BubblePadding = 2f;
    public static int GameWindowHeight = 1080;
    public const float BubbleSpeed = 25f;
    public static SpriteBatch SpriteBatch;
    public static ContentManager Content;
    public static GraphicsDevice GraphicsDevice;
    public static BubbleManager BubbleManager;
    public static CardManager CardManager;
    public static Shooter Shooter;
    public static Texture2D Pixel;
    public static Texture2D ShooterSceneSheet;
    public static Texture2D CardSheet;
    public static SpriteFont Font;
    public static GameState GameState;
    public static PlayerState PlayerState;
    public static float Time { get; private set; }
    public static Color BlueColor = new Color(84, 161, 185);

    public static Color DarkBlueColor = new Color(44, 120, 143);

    public static Color DarkerBlueColor = new Color(40, 71, 81);

    public static Color FreezeColor = new Color(127, 220, 220);

    public readonly Random Random = new Random();
    public static SoundEffect PopSound;

    public static SoundEffect CollideSound;

    public static SoundEffect DropSound;

    public static SoundEffect WinSound;

    public static SoundEffect LoseSound;

    public static SoundEffect ExplodeSound;

    public static SoundEffect FreezeSound;

    public static SoundEffect RainbowSound;

    public static SoundEffect MagicSound;

    public static SoundEffect PickCardSound;

    public static SoundEffect ExtraLifeConsumeSound;

    public static Song backgroundMusic;

    

    public static SoundEffectInstance PopSoundInstance;

    public static SoundEffectInstance CollideSoundInstance;

    public static SoundEffectInstance DropSoundInstance;

    public static SoundEffectInstance WinSoundInstance;

    public static SoundEffectInstance LoseSoundInstance;

    public static SoundEffectInstance ExplodeSoundInstance;

    public static SoundEffectInstance FreezeSoundInstance;

    public static SoundEffectInstance RainbowSoundInstance;

    public static SoundEffectInstance MagicSoundInstance;

    public static SoundEffectInstance PickCardSoundInstance;

    public static SoundEffectInstance ExtraLifeConsumeSoundInstance;

    public static bool Muted = false;
    private Globals()
    {
    }

    public static Globals instance;

    public static Globals Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Globals();
            }
            return instance;
        }
    }

    public static void Update(GameTime gt)
    {
        Time = (float)gt.ElapsedGameTime.TotalSeconds;
        InputManager.Update();
    }

    public static RenderTarget2D GetNewRenderTarget()
    {
        return new(GraphicsDevice, ScreenW, ScreenH);
    }

    public static void MuteAllSoundVolumes()
    {
        PopSoundInstance.Volume = 0f;
        CollideSoundInstance.Volume = 0f;
        DropSoundInstance.Volume = 0f;
        WinSoundInstance.Volume = 0f;
        LoseSoundInstance.Volume = 0f;
        ExplodeSoundInstance.Volume = 0f;
        FreezeSoundInstance.Volume = 0f;
        RainbowSoundInstance.Volume = 0f;
        MagicSoundInstance.Volume = 0f;
        PickCardSoundInstance.Volume = 0f;
        ExtraLifeConsumeSoundInstance.Volume = 0f;
    }
    public static void UnmuteAllSoundVolumes()
    {
        PopSoundInstance.Volume = 1f;
        CollideSoundInstance.Volume = 1f;
        DropSoundInstance.Volume = 0.3f;
        WinSoundInstance.Volume = 1f;
        LoseSoundInstance.Volume = 1f;
        ExplodeSoundInstance.Volume = 1f;
        FreezeSoundInstance.Volume = 1f;
        RainbowSoundInstance.Volume = 1f;
        MagicSoundInstance.Volume = 1f;
        PickCardSoundInstance.Volume = 1f;
        ExtraLifeConsumeSoundInstance.Volume = 1f;
    }

}