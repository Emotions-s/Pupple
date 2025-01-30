using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Managers;

namespace Pupple.Objects.Scenes;
public abstract class Scene
{
    protected readonly RenderTarget2D target;
    protected readonly GameManager game;

    public Scene(GameManager gameManager)
    {
        game = gameManager;
        target = Globals.GetNewRenderTarget();
        Load();
    }

    protected abstract void Load();
    protected abstract void Draw();
    public abstract void Update();
    public abstract void Activate();

    public virtual RenderTarget2D GetFrame()
    {
        Globals.GraphicsDevice.SetRenderTarget(target);
        Globals.GraphicsDevice.Clear(Color.Black);
        Draw();
        Globals.GraphicsDevice.SetRenderTarget(null);
        return target;
    }
}