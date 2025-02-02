using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Managers;
using Pupple.States;

namespace Pupple;

class Globals
{
    public const int ScreenW = 1920;
    public const int ScreenH = 1080;
    public const int GridSize = 60;
    public const float BubbleRadius = 28f;
    public const float BubblePadding = 2f;
    public const float BubbleSpeed = 20f;
    public static SpriteBatch SpriteBatch { get; set; }
    public static ContentManager Content { get; set; }
    public static GraphicsDevice GraphicsDevice { get; set; }
    public static BubbleManager BubbleManager { get; set; }
    public static Shooter Shooter { get; set; }
    public static Texture2D Pixel { get; set; }
    public static Texture2D BubbleTexture { get; set; }
    public static SpriteFont Font { get; set; }
    public static GameState GameState { get; set; }
    public static PlayerState PlayerState { get; set; }
    public static float Time { get; private set; }

    public static Color BlueColor = new Color(84, 161, 185);

    public static Color DarkBlueColor = new Color(44, 120, 143);

    public static Color DarkerBlueColor = new Color(40, 71, 81);

    public readonly Random random = new Random();
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
}