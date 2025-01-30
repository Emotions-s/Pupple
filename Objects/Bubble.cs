using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Objects.Scenes;

namespace Pupple.Objects;
public abstract class Bubble : IComponent
{
    private readonly Texture2D _texture;
    public Vector2 Position { get; set; }

    public bool IsActive { get; set; }

    public bool IsMoving { get; set; }

    public Rectangle Viewport { get; set; }

    public Vector2 Velocity { get; set; }

    public float Radius { get; set; }

    public Bubble(Vector2 pos, Texture2D texture)
    {
        Position = pos;
        _texture = texture;
        IsActive = true;
    }

    public void Update()
    {
        Position += Velocity;

        if (IsMoving && !IsInWindow())
        {
            IsActive = false;
        }
    }

    public void Draw()
    {
        Globals.SpriteBatch.Draw(
            _texture,
            Position,
            Viewport,
            Color.White,
            0f,
            new Vector2(Globals.BubbleRadius, Globals.BubbleRadius),
            1f,
            SpriteEffects.None,
            0f
        );
    }

    private bool IsInWindow()
    {
        float leftSidePos = Position.X + PlayScene.GameWindowOffset - Globals.BubbleRadius;
        float rightSidePos = Position.X + PlayScene.GameWindowOffset + Globals.BubbleRadius;
        float topSidePos = Position.Y - Globals.BubbleRadius;

        if (Common.IsInGameWindow(new Vector2(leftSidePos, topSidePos)) && Common.IsInGameWindow(new Vector2(rightSidePos, topSidePos)))
        {
            return true;
        }
        return false;
    }

    public void ShiftDown(float distance)
    {
        Position = new Vector2(Position.X, Position.Y + distance);
    }

}