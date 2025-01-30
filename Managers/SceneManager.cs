using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Objects.Scenes;

namespace Pupple.Managers;

public class SceneManager
{
    public ScenesType ActiveScene { get; private set; }
    private readonly Dictionary<ScenesType, Scene> _scenes = new();

    public SceneManager(GameManager gameManager)
    {
        _scenes.Add(ScenesType.PlayScene, new PlayScene(gameManager));

        ActiveScene = ScenesType.PlayScene;
        _scenes[ActiveScene].Activate();
    }

    public void SwitchScene(ScenesType scene)
    {
        ActiveScene = scene;
        _scenes[ActiveScene].Activate();
    }

    public void Update()
    {
        _scenes[ActiveScene].Update();
    }

    public RenderTarget2D GetFrame()
    {
        return _scenes[ActiveScene].GetFrame();
    }
}