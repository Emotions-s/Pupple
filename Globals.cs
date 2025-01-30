using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Managers;

namespace Pupple;

class Globals
{
    public const int ScreenW = 1920;
    public const int ScreenH = 1080;
    public const int GridSize = 60;
    public const float BubbleRadius = 28f;
    public const float BubblePadding = 2f;
    public const int BubbleQueueMaxSize = 4;
    public static SpriteBatch SpriteBatch { get; set; }

    public static ContentManager Content { get; set; }
    public static GraphicsDevice GraphicsDevice { get; set; }

    public static Texture2D Pixel { get; set; }

    public static Texture2D BubbleTexture { get; set; }

    public static float Time { get; private set; }

    public int Coin = 0;

    public int Level = 1;

    public int CurBubbleQueueSize = 0;
    public int IgnorePercent = 50;

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