using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pupple.Managers;
using Pupple.States;

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
        ExtraLifeBox extraLifeBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 3), "Extra Life", Color.White, Globals.DarkBlueColor, Globals.ShooterSceneSheet, BubbleHelper.ActiveShieldViewPort, BubbleHelper.InactiveShieldViewPort);
        MissCountBox missCountBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 7), "Miss Count", Globals.DarkerBlueColor, Color.White, Globals.ShooterSceneSheet, BubbleHelper.ActiveMissViewPort, BubbleHelper.InactiveMissViewPort);
        FreezeTimeBox freezeTimeBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 9), "Freeze", Color.LightGray, Globals.DarkerBlueColor);
 
        Button muteButton = new Button(
            new Vector2(Globals.GridSize * 1, Globals.GridSize * 16),  // Position
            Globals.ShooterSceneSheet,  // Texture (the full sprite sheet)
            new Rectangle(216, 360, 60, 60),  // Initial Source Rectangle (Muted Button sprite)
            null   
        );

        muteButton.onClickAction = () =>
        {
            if (Globals.Muted)
            {
                // Unmute
                MediaPlayer.Volume = 0.3f;  // Restore music volume
                Globals.UnmuteAllSoundVolumes();  // Unmute all sound effects
                Globals.Muted = false;  // Set the mute flag to false
                muteButton.sourceRectangle = new Rectangle(216, 360, 120, 60);  // Unmuted Button sprite
            }
            else
            {
                // Mute
                MediaPlayer.Volume = 0;  // Mute the music
                Globals.MuteAllSoundVolumes();  // Mute all sound effects
                Globals.Muted = true;  // Set the mute flag to true
                muteButton.sourceRectangle = new Rectangle(336, 360, 60, 60);  // Muted Button sprite
            }
        };

        Button exitButton = new Button(
            new Vector2(Globals.GridSize * 3, Globals.GridSize * 16),  // Position
            Globals.ShooterSceneSheet,  // Texture (the full sprite sheet)
            new Rectangle(456, 360, 120, 60),  // Source rectangle (position and size in the sprite sheet)
            () =>
                {
                    // Action to exit the game
                    Environment.Exit(0);  // Exits the application
                }
        );

        Window leftWindow = new(SideBarWidth, Globals.ScreenH, new(LeftBarOffset, 0), Globals.BlueColor)
        {
            Components = [
                levelBox,
                extraLifeBox,
                missCountBox,
                freezeTimeBox,
                muteButton,
                exitButton
            ],
        };

        // game window
        Globals.BubbleManager = new BubbleManager(17, 20);
        Globals.Shooter = new(
            Globals.ShooterSceneSheet,
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
        BombBox bombBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 3), "Bomber Ball", Color.White, Globals.DarkerBlueColor, Globals.ShooterSceneSheet);
        FreezeBox freezeBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 6), "Freeze Ball", Color.White, Globals.DarkerBlueColor, Globals.ShooterSceneSheet);
        RainbowBox rainbowBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 9), "Rainbow Ball", Color.White, Globals.DarkerBlueColor, Globals.ShooterSceneSheet);
        MagicBox magicBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 12), "Magic Ball", Color.White, Globals.DarkerBlueColor, Globals.ShooterSceneSheet);
        QueueBox queueBox = new(Globals.GridSize * 6, Globals.GridSize * 2, new Vector2(0, Globals.GridSize * 15), "Queue", Globals.DarkerBlueColor, Color.White, Globals.ShooterSceneSheet);

        Window rightWindow = new(SideBarWidth, Globals.ScreenH, new(RightBarOffset, 0), Globals.BlueColor)
        {
            Components = [
                textBox,
                bombBox,
                freezeBox,
                rainbowBox,
                magicBox,
                queueBox
            ],
        };

        _windows = [
            leftWindow,
            gameWindow,
            rightWindow,
        ];
        _windowTargets = new RenderTarget2D[_windows.Length];
        Globals.CardManager = new CardManager();
    }
    public override void Reset()
    {
        Globals.GameState.Reset();
        Globals.PlayerState.Reset();
        for (int i = 0; i < _windows.Length; i++)
        {
            _windows[i].Reset();
        }
    }

    public override void Update()
    {
        // Debug tools
        HandleDebug();
        if (Globals.GameState.CurrentState == GameState.State.GameOver)
        {
            if (InputManager.KeyPressed(Keys.Space))
            {
                Reset();
            }
            return;
        }
        if (Globals.GameState.CurrentState == GameState.State.Playing)
        {
            if (Globals.BubbleManager.IsPassTheStage())
            {
                ChangeGameState(GameState.State.Shop);
                return;
            }
        }

        for (int i = 0; i < _windows.Length; i++)
        {
            _windows[i].Update();
        }
        Globals.GameState.FreezeTime -= (float)Globals.Time;
        if (Globals.GameState.FreezeTime <= 0)
        {
            Globals.GameState.FreezeTime = 0;
        }
        if (Globals.GameState.CurrentState == GameState.State.Shop)
        {
            Globals.CardManager.Update();
        }
    }

    protected override void Draw()
    {
        for (int i = 0; i < _windows.Length; i++)
        {
            Globals.SpriteBatch.Draw(_windowTargets[i], _windows[i].OriginPos, Color.White);
        }
        if (Globals.GameState.CurrentState == GameState.State.GameOver)
        {
            ShowDeadScreen();
        }
        if (Globals.GameState.CurrentState == GameState.State.Shop)
        {
            Globals.CardManager.Draw();
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

    public static void ChangeGameState(GameState.State state)
    {
        Globals.GameState.CurrentState = state;
        if (state == GameState.State.Shop)
        {
            Globals.CardManager.DrawCards();
            Globals.GameState.CurrentState = GameState.State.Shop;
        }
        else if (state == GameState.State.Playing)
        {
            Globals.GameState.LevelUp();
            Globals.BubbleManager.Reset();
            Globals.Shooter.Reset();
            Globals.GameState.CurrentState = GameState.State.Playing;
        }
    }

    private void ShowDeadScreen()
    {
        Globals.SpriteBatch.Draw(Globals.Pixel, new Rectangle(0, 0, Globals.ScreenW, Globals.ScreenH), Color.Black * 0.8f);
        var text = "You died!";
        var textSize = Globals.Font.MeasureString(text);
        Globals.SpriteBatch.DrawString(Globals.Font,
            text,
            new Vector2(Globals.ScreenW / 2, 300),
            Color.Red,
            0f,
            textSize / 2,
            1.5f,
            SpriteEffects.None,
            0f
        );

        text = "Your highest level is " + Globals.GameState.Level;
        textSize = Globals.Font.MeasureString(text);
        Globals.SpriteBatch.DrawString(Globals.Font,
            text,
            new Vector2(Globals.ScreenW / 2, 400),
            Color.White,
            0f,
            textSize / 2,
            1f,
            SpriteEffects.None,
            0f
        );

        text = "Press Space to restart";
        textSize = Globals.Font.MeasureString(text);
        Globals.SpriteBatch.DrawString(Globals.Font,
            text,
            new Vector2(Globals.ScreenW / 2, 800),
            Color.White,
            0f,
            textSize / 2,
            0.75f,
            SpriteEffects.None,
            0f
        );
    }

    private void HandleDebug()
    {
        if (InputManager.KeyPressed(Keys.F2))
        {
            Globals.BubbleManager.AddNewTopLine();
            return;
        }

        if (InputManager.KeyPressed(Keys.F3))
        {
            ChangeGameState(GameState.State.Shop);
            return;
        }

        if (InputManager.KeyPressed(Keys.F4))
        {
            ChangeGameState(GameState.State.Playing);
            return;
        }
    }
}