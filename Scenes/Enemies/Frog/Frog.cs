using Godot;
using System;

public partial class Frog : EnemyBase
{
    private const float JumpMinTime = 2.0f;
    private const float JumpMaxTime = 3.0f;
    private const float JumpVelocityX = 100f;
    private const float JumpVelocityY = 150f;
    
    private static readonly Vector2 JumpVelocityRight = new Vector2(JumpVelocityX, -JumpVelocityY);
    private static readonly Vector2 JumpVelocityLeft = new Vector2(-JumpVelocityX, -JumpVelocityY);
    
    [Export] private Timer _jumpTimer;
    
    private bool _seenPlayer = false;
    private bool _jump = false;
    
    public override void _Ready()
    {
        base._Ready();
        _jumpTimer.Timeout += OnJumpTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (!IsOnFloor())
        {
            Velocity += new Vector2(0f, _gravity * (float) delta);
        }
        else
        {
            Velocity = Vector2.Zero;
            _animatedSprite2D.Play("idle"); //playing an animation that is already playing does nothing. So no need to worry about calling this all the time
        }
        
        ApplyJump();
        MoveAndSlide();
        FlipMe();
    }

    protected override void OnScreenEntered()
    {
        if (!_seenPlayer)
        {
            _seenPlayer = true;
            StartTimer();
        }
    }

    private void FlipMe()
    {
        if (IsOnFloor())
        {
            _animatedSprite2D.FlipH = _playerRef.GlobalPosition.X > GlobalPosition.X;
        }
    }

    private void ApplyJump()
    {
        if (IsOnFloor() && _seenPlayer && _jump)
        {
            _jump = false;
            _animatedSprite2D.Play("jump");
            Velocity = _animatedSprite2D.FlipH ? JumpVelocityRight : JumpVelocityLeft;
            StartTimer();
        }
    }

    private void StartTimer()
    {
        _jumpTimer.WaitTime = GD.RandRange(JumpMinTime, JumpMaxTime);
        _jumpTimer.Start();
    }

    private void OnJumpTimerTimeout()
    {
        _jump = true;
    }
}
