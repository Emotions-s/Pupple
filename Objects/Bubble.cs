using System;
using System.Collections.Generic;
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

    public Bubble(Vector2 pos, Texture2D texture)
    {
        Position = pos;
        _texture = texture;
        Reset();
    }

    public void Reset()
    {
        Velocity = Vector2.Zero;
        IsActive = true;
        IsMoving = false;
    }

    public void Update()
    {
        // check to bounce off walls

        Position += Velocity;

        if (Position.X < Globals.BubbleRadius || Position.X > PlayScene.GameWindowWidth - Globals.BubbleRadius)
        {
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
        }

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

    public bool IsColliding(Bubble bubble)
    {
        float distance = Vector2.Distance(Position, bubble.Position);
        return distance <= Globals.BubbleRadius * 2;
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