using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple;
using Pupple.Managers;
using Pupple.Objects;
using Pupple.States;

public class Shooter : IComponent
{
    private Texture2D _texture;

    private Vector2 _position;

    private readonly Vector2 __positionOrigin;

    private Vector2 _windowOrigin;

    private Rectangle _viewport;

    private float _angle;
    private readonly float _minAngle;
    private readonly float _maxAngle;
    private Vector2 _origin;

    // Movement & Shooting
    private float _moveSpeed;
    private float _shootSpeed;

    public Bubble[] BubbleQueue { get; private set; }

    public Shooter(Texture2D texture,
                    Vector2 position,
                    Vector2 windowOrigin)
    {
        _texture = texture;
        __positionOrigin = position;
        _windowOrigin = windowOrigin;

        _minAngle = -MathHelper.Pi + 0.0001f;
        _maxAngle = 0 - 0.0001f;

        _viewport = new Rectangle(0, 360, 156, 156);
        _origin = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);

        Reset();
    }

    public void Reset()
    {
        _position = __positionOrigin;
        _moveSpeed = 200f;
        _shootSpeed = Globals.BubbleSpeed;
        _angle = 0f;
        BubbleQueue = new Bubble[PlayerState.MaxBubbleQueueSize + 1];
        Reload();
    }
    public void Update()
    {
        if (Globals.GameState.CurrentState != GameState.State.Playing) return;

        float delta = (float)Globals.Time;
        if (InputManager.IsLeftHeld)
        {
            _position.X -= _moveSpeed * delta;
        }
        if (InputManager.IsRightHeld)
        {
            _position.X += _moveSpeed * delta;
        }

        _position.X = MathHelper.Clamp(_position.X, 0, 1200);

        if (BubbleQueue[0]?.IsMoving == false)
        {
            BubbleQueue[0].Position = _position;
        }

        RotateToMouse();
        if (InputManager.Clicked && Common.IsInGameWindow(InputManager.MousePosition))
        {
            ShootCurrentBubble();
        }
        BubbleQueue[0]?.Update();

        if (BubbleQueue[0]?.IsMoving == true)
        {
            Globals.BubbleManager.HandleShotBubble(BubbleQueue[0]);
        }
    }

    public void Draw()
    {
        Globals.SpriteBatch.Draw(
            _texture,
            _position,
            _viewport,
            Color.White,
            _angle,
            _origin,
            1f,
            SpriteEffects.None,
            0f
        );

        BubbleQueue[0]?.Draw();
    }

    private void RotateToMouse()
    {
        Vector2 mousePos = InputManager.MousePosition;
        Vector2 globalShooterPos = _windowOrigin + _position;
        Vector2 direction = mousePos - globalShooterPos;

        float desiredAngle = (float)Math.Atan2(direction.Y, direction.X);
        _angle = MathHelper.Clamp(desiredAngle, _minAngle, _maxAngle);
    }


    private Bubble GenerateRandomBubble()
    {
        BubbleColor randomColor = Common.GetRandomElement(Globals.GameState.BubbleColorsInGame);
        Bubble bubble = new NormalBubble(_position, Globals.ShooterSceneSheet, randomColor);
        return bubble;
    }

    public void SwitchBubble(int index)
    {
        if (BubbleQueue[index] == null || BubbleQueue[0] == null || BubbleQueue[0].IsMoving) return;

        Bubble temp = BubbleQueue[0];
        BubbleQueue[0] = BubbleQueue[index];
        BubbleQueue[index] = temp;
    }

    public void ChangeBubble(int index, Bubble bubble)
    {
        if (BubbleQueue[index] == null || BubbleQueue[0] == null || BubbleQueue[0].IsMoving) return;

        bubble.Position = _position;
        BubbleQueue[index] = bubble;
    }

    public void Reload()
    {
        BubbleQueue[0] = null;

        while (BubbleQueue[0] == null)
        {
            for (int i = 0; i < Globals.PlayerState.CurrentBubbleQueueSize; i++)
            {
                BubbleQueue[i] = BubbleQueue[i + 1];
            }
            BubbleQueue[Globals.PlayerState.CurrentBubbleQueueSize] = GenerateRandomBubble();
        }
        BubbleQueue[0].Position = _position;
    }

    private void ShootCurrentBubble()
    {
        if (BubbleQueue[0] == null || BubbleQueue[0].IsMoving) return;

        BubbleQueue[0].Velocity = new Vector2(
            (float)Math.Cos(_angle),
            (float)Math.Sin(_angle)
        ) * _shootSpeed;
        BubbleQueue[0].IsMoving = true;

    }
}