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

    // Pop animation variables
    public bool IsPopping { get; private set; } = false;
    private float _scale = 1f;
    private const float PopAnimationSpeed = 3f;

    // Floating behavior
    public bool IsFloating { get; private set; } = false;
    private float floatSpeed = -100f; // Moves up first
    private float fallSpeed = 3000f;  // Falls down after delay
    private float floatTime = 0.3f;  // Time before falling
    private float elapsedTime = 0f; 

    public Bubble(Vector2 pos, Texture2D texture)
    {
        Position = pos;
        _texture = texture;
         _scale = 1f; // Reset scale
        Reset();
    }

    public void Reset()
    {
        Velocity = Vector2.Zero;
        IsActive = true;
        IsMoving = false;
    }
    public void StartPop()
    {
        IsPopping = true;
    }

    public void StartFloating()
    {
        IsFloating = true;
        elapsedTime = 0f;  // Reset time counter
    }
    public void Update()
    {
        //if popping bubble shrink
        if (IsPopping)
        {
            _scale -= PopAnimationSpeed * (float)Globals.Time;
            if (_scale <= 0)
            {
                _scale = 0;
                IsActive = false; // Mark the bubble as inactive
            }
        }
        else if (IsFloating)
        {
            elapsedTime += (float)Globals.Time;

            if (elapsedTime < floatTime)
            {
                Position += new Vector2(0, floatSpeed * (float)Globals.Time); // Move up
            }
            else
            {
                Position += new Vector2(0, fallSpeed * (float)Globals.Time); // Fall down
                if (Position.Y > Globals.GameWindowHeight + Globals.BubbleRadius)
                {
                    IsActive = false; // Remove bubble when it falls off the screen
                }
            }
        }
        else
        {
            // Increase velocity after bounce to ensure faster movement
            float bounceSpeedMultiplier = 1.05f; // Adjust this to make it bounce faster

            Position += Velocity * (float)Globals.Time * 2f; // Speed up the movement

            // Bounce off walls properly and keep moving fast
            if (Position.X <= Globals.BubbleRadius || Position.X >= PlayScene.GameWindowWidth - Globals.BubbleRadius)
            {
                Velocity = new Vector2(-Velocity.X * bounceSpeedMultiplier, Velocity.Y); // Reverse X direction with speed increase
            }

            //Ensure the ball keeps moving properly without getting stuck
            if (IsMoving && !IsInWindow())
            {
                if (Position.Y <= Globals.BubbleRadius)
                {
                    // StartPop();
                    IsActive = false;
                }
            }
        }
    }

    public void Draw()
    {
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