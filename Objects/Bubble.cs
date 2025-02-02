using System;
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

    // Pop animation variables
    public bool IsPopping { get; private set; } = false;
    private float _scale = 1f;
    private const float PopAnimationSpeed = 3f;

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
        _scale = 1f; // Reset scale
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
        //if popping bubble shrink
        if (IsPopping)
        {
            _scale -= PopAnimationSpeed * (float)Globals.Time;
            if (_scale <= 0)
            {
                IsActive = false; // Bubble disappears after shrinking
            }
        }
        else
        {
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
    }
    public void StartPop()
    {
        IsPopping = true;
    }

    public void Draw()
    {
        // Globals.SpriteBatch.Draw(
        //     _texture,
        //     Position,
        //     Viewport,
        //     Color.White,
        //     0f,
        //     new Vector2(Globals.BubbleRadius, Globals.BubbleRadius),
        //     1f,
        //     SpriteEffects.None,
        //     0f
        // );
        if (IsActive)
        {
            Globals.SpriteBatch.Draw(
                _texture,
                Position,
                Viewport,
                Color.White,
                0f,
                new Vector2(Globals.BubbleRadius, Globals.BubbleRadius),
                _scale, // Apply scale animation
                SpriteEffects.None,
                0f
            );
        }
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