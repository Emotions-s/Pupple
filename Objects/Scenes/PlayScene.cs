using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Managers;

namespace Pupple.Objects.Scenes;

public class PlayScene(GameManager gameManager) : Scene(gameManager)
{
    public const int SideBarWidth = 360;
    public const int GameWindowWidth = 1200;

    public const int LeftBarOffset = 0;
    public const int GameWindowOffset = SideBarWidth;
    public const int RightBarOffset = SideBarWidth + GameWindowWidth;

    private Window[] _windows;

    private RenderTarget2D[] _windowTargets;

    protected override void Load()
    {
        // left window
        LevelBox levelBox = new(Globals.GridSize * 6, Globals.GridSize * 1, new Vector2(0, Globals.GridSize * 1), null, Globals.DarkerBlueColor, Color.White);

        ExtraLifeBox extraLifeBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 3), "Extra Life", Color.White, Globals.DarkBlueColor, Globals.BubbleTexture, BubbleHelper.NormalBubbleViewPort[BubbleColor.Green], BubbleHelper.NormalBubbleViewPort[BubbleColor.Red]);

        MissCountBox missCountBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 7), "Miss Count", Globals.DarkBlueColor, Color.White, Globals.BubbleTexture, BubbleHelper.NormalBubbleViewPort[BubbleColor.Red], BubbleHelper.NormalBubbleViewPort[BubbleColor.White]);

        Window leftWindow = new(SideBarWidth, Globals.ScreenH, new(LeftBarOffset, 0), Globals.BlueColor)
        {
            Components = [
                levelBox,
                extraLifeBox,
                missCountBox
            ],
        };

        // game window
        Globals.BubbleManager = new BubbleManager(17, 20);
        Texture2D shooterTex = Globals.Content.Load<Texture2D>("objects/Shooter");
        Globals.Shooter = new(shooterTex,
            new(1200 / 2, 1000),
            new Vector2(360, 0)
        );

        Window gameWindow = new(GameWindowWidth, Globals.ScreenH, new(GameWindowOffset, 0), Globals.DarkBlueColor)
        {
            Components = [
                Globals.BubbleManager,
                Globals.Shooter
            ],
        };

        // right window
        TextBox textBox = new(Globals.GridSize * 6, Globals.GridSize * 1, new Vector2(0, Globals.GridSize * 1), "Special Ball", Globals.DarkerBlueColor, Color.White);
        QueueBox queueBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 15), "Queue", Globals.DarkerBlueColor, Color.White, Globals.BubbleTexture);

        Window rightWindow = new(SideBarWidth, Globals.ScreenH, new(RightBarOffset, 0), Globals.BlueColor)
        {
            Components = [
                textBox,
                queueBox
            ],
        };

        _windows = [
            leftWindow,
            gameWindow,
            rightWindow,
        ];
        _windowTargets = new RenderTarget2D[_windows.Length];
    }
    public override void Activate()
    {
        if (Globals.GameState.IsDead)
        {
            Globals.GameState.Reset();
            Globals.PlayerState.Reset();
            for (int i = 0; i < _windows.Length; i++)
            {
                _windows[i].Reset();
            }
        }
    }

    public override void Update()
    {
        for (int i = 0; i < _windows.Length; i++)
        {
            _windows[i].Update();
        }
        Globals.GameState.FreezeTime -= (float)Globals.Time;
        if (Globals.GameState.FreezeTime <= 0)
        {
            Globals.GameState.FreezeTime = 0;
        }
    }

    protected override void Draw()
    {
        for (int i = 0; i < _windows.Length; i++)
        {
            Globals.SpriteBatch.Draw(_windowTargets[i], _windows[i].OriginPos, Color.White);
        }
    }

    public override RenderTarget2D GetFrame()
    {
        for (int i = 0; i < _windows.Length; i++)
        {
            _windowTargets[i] = _windows[i].GetFrame();
        }

        Globals.GraphicsDevice.SetRenderTarget(target);
        Globals.GraphicsDevice.Clear(Color.Black);
        Globals.SpriteBatch.Begin();
        Draw();
        Globals.SpriteBatch.End();
        Globals.GraphicsDevice.SetRenderTarget(null);
        return target;
    }
}