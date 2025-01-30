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
    private BubbleManager _bubbleManager;

    private Window[] _windows;

    private RenderTarget2D[] _windowTargets;

    protected override void Load()
    {
        // left window
        Window leftWindow = new(SideBarWidth, Globals.ScreenH, new(LeftBarOffset, 0), new Color(84, 161, 185))
        {

        };

        // game window

        _bubbleManager = new BubbleManager(17, 20);

        Texture2D shooterTex = Globals.Content.Load<Texture2D>("objects/Shooter");

        Shooter shooter = new(shooterTex,
            new(1200 / 2, 1000),
            new Vector2(360, 0)
        );

        Window gameWindow = new(GameWindowWidth, Globals.ScreenH, new(GameWindowOffset, 0), new Color(44, 120, 143))
        {
            Components = [
                _bubbleManager,
                shooter
            ],
        };

        // right window
        Window rightWindow = new(SideBarWidth, Globals.ScreenH, new(RightBarOffset, 0), new Color(84, 161, 185))
        {

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
    }

    public override void Update()
    {
        // _bubbleManager.Update();
        for (int i = 0; i < _windows.Length; i++)
        {
            _windows[i].Update();
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