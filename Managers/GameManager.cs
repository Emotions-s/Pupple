using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pupple.Objects.Scenes;

namespace Pupple.Managers;

public class GameManager
{
    private readonly SceneManager _sceneManager;
    public GameManager()
    {
        _sceneManager = new SceneManager(this);
    }

    public void Update()
    {
        _sceneManager.Update();
        if (InputManager.KeyPressed(Keys.F1))
        {
            _sceneManager.SwitchScene(ScenesType.PlayScene);
        }
    }

    public void Draw()
    {
        var f = _sceneManager.GetFrame();
        Globals.SpriteBatch.Begin();
        Globals.SpriteBatch.Draw(f, Vector2.Zero, Color.White);
        Globals.SpriteBatch.End();
    }
}