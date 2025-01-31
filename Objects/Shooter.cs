using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple;
using Pupple.Managers;
using Pupple.Objects;

public class Shooter : IComponent
{
    private Texture2D _texture;

    private Vector2 _position;

    private readonly Vector2 __positionOrigin;

    private Vector2 _windowOrigin;

    private float _angle;
    private readonly float _minAngle;
    private readonly float _maxAngle;
    private Vector2 _origin;

    // Movement & Shooting
    private float _moveSpeed;
    private float _shootSpeed;

    private Bubble[] _bubbleQueue;

    public Shooter(Texture2D texture,
                    Vector2 position,
                    Vector2 windowOrigin)
    {
        _texture = texture;
        __positionOrigin = position;
        _windowOrigin = windowOrigin;

        _minAngle = -MathHelper.Pi + 0.0001f;
        _maxAngle = 0 - 0.0001f;
        _origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);

        Reset();
    }

    public void Reset()
    {
        _position = __positionOrigin;
        _moveSpeed = 200f;
        _shootSpeed = Globals.BubbleSpeed;
        _angle = 0f;
        _bubbleQueue = new Bubble[Globals.BubbleQueueMaxSize + 1];
        Reload();
    }
    public void Update()
    {
        float delta = (float)Globals.Time;
        if (InputManager.IsLeftHeld)
        {
            if (_bubbleQueue[0]?.IsMoving == false)
                _bubbleQueue[0].Position = new Vector2(_bubbleQueue[0].Position.X - _moveSpeed * delta, _bubbleQueue[0].Position.Y);
            _position.X -= _moveSpeed * delta;
        }
        if (InputManager.IsRightHeld)
        {
            if (_bubbleQueue[0]?.IsMoving == false)
                _bubbleQueue[0].Position = new Vector2(_bubbleQueue[0].Position.X + _moveSpeed * delta, _bubbleQueue[0].Position.Y);
            _position.X += _moveSpeed * delta;
        }

        _position.X = MathHelper.Clamp(_position.X, 0, 1200);

        RotateToMouse();
        if (InputManager.Clicked && Common.IsInGameWindow(InputManager.MousePosition))
        {
            ShootCurrentBubble();
        }
        _bubbleQueue[0]?.Update();

        if (_bubbleQueue[0]?.IsMoving == true)
        {
            Globals.BubbleManager.HandleShotBubble(_bubbleQueue[0]);
        }
    }

    public void Draw()
    {
        Globals.SpriteBatch.Draw(
            _texture,
            _position,
            null,
            Color.White,
            _angle,
            _origin,
            1f,
            SpriteEffects.None,
            0f
        );

        for (int i = 0; i < Globals.Instance.CurBubbleQueueSize + 1; i++)
        {
            _bubbleQueue[i]?.Draw();
        }
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
        BubbleColor randomColor = Common.GetRandomElement(BubbleHelper.BubbleColorsLv1);
        Bubble bubble = new NormalBubble(_position, Globals.BubbleTexture, randomColor);
        return bubble;
    }

    public void Reload()
    {
        // if (_bubbleQueue[0] != null) return;

        _bubbleQueue[0] = null;

        while (_bubbleQueue[0] == null)
        {
            for (int i = 0; i < Globals.Instance.CurBubbleQueueSize; i++)
            {
                _bubbleQueue[i] = _bubbleQueue[i + 1];
            }
            _bubbleQueue[Globals.Instance.CurBubbleQueueSize] = GenerateRandomBubble();
        }
    }

    private void ShootCurrentBubble()
    {
        if (_bubbleQueue[0] == null || _bubbleQueue[0].IsMoving) return;

        _bubbleQueue[0].Velocity = new Vector2(
            (float)Math.Cos(_angle),
            (float)Math.Sin(_angle)
        ) * _shootSpeed;
        _bubbleQueue[0].IsMoving = true;

    }
}