using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const string GroupName = "Player";
	
	private enum PlayerState { Idle, Run, Jump, Fall, Hurt }
	
	private const float Gravity = 690.0f;
	private const float JumpVelocity = -260.0f;
	private const float MaxFall = 400.0f;
	private const float RunSpeed = 120.0f;

	private static readonly Vector2 HurtJump = new Vector2(0, -130.0f);

	[Export] private float _yFallOff = 100.0f;
	[Export] private Sprite2D _sprite2D;
	[Export] private AudioStreamPlayer2D _soundPlayer;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Label _debugLabel;
	[Export] private Shooter _shooter;
	[Export] private Timer _invincibleTimer;
	[Export] private Timer _hurtTimer;
	[Export] private AnimationPlayer _invincibleAnimationPlayer;
	[Export] private Area2D _hitBox;
	[Export] private int _lives = 3;
	
	private PlayerState _state = PlayerState.Idle;
	private bool _invincible = false;

	public override void _Ready()
	{
		_invincibleTimer.Timeout += OnInvincibleTimerTimeout;
		_hurtTimer.Timeout += OnHurtTimerTimeout;
		_hitBox.AreaEntered += OnHitBoxAreaEntered;

		CallDeferred(MethodName.LateInit);
	}

	private void LateInit()
	{
		SignalManager.EmitOnLevelStart(_lives);
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = GetInput((float)delta);
		MoveAndSlide();
		CalculateState();
		Shoot();
		UpdateDebugLabel();
		FallenOff();
	}

	private void UpdateDebugLabel()
	{
		string debugText = "";
		debugText += $"floor:{IsOnFloor()}\n";
		debugText += $"floor:{_state}\n";
		debugText += $"{Velocity.X:0f},{Velocity.Y:0f}\n";
		_debugLabel.Text = debugText;
	}

	private void Shoot()
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			_shooter.Shoot(_sprite2D.FlipH ? Vector2.Left : Vector2.Right);
		}
	}

	private void FallenOff()
	{
		if (GlobalPosition.Y < _yFallOff)
		{
			return;
		}

		_lives = 1;
		ReduceLives();
	}

	private Vector2 GetInput(float delta)
	{
		Vector2 newVelocity = Velocity;
		newVelocity.X = 0;
		newVelocity.Y += Gravity * delta;
		
		if (_state == PlayerState.Hurt) return newVelocity;

		if (Input.IsActionPressed("left"))
		{
			newVelocity.X = -RunSpeed;
			_sprite2D.FlipH = true;
		}

		if (Input.IsActionPressed("right"))
		{
			newVelocity.X = RunSpeed;
			_sprite2D.FlipH = false;
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			newVelocity.Y = JumpVelocity;
			SoundManager.PlayClip(_soundPlayer, SoundManager.SoundJump);
		}
		newVelocity.Y = Mathf.Clamp(newVelocity.Y, JumpVelocity, MaxFall);
		
		return newVelocity;
	}

	private void OnHitBoxAreaEntered(Area2D area)
	{
		ApplyHit();
	}

	private void ApplyHurtJump()
	{
		Velocity = HurtJump;
		_animationPlayer.Play("hurt");
		_hurtTimer.Start();
	}

	private bool ReduceLives()
	{
		_lives--;
		GD.Print("Lives: " + _lives);
		SignalManager.EmitOnPlayerHit(_lives);
		if (_lives <= 0)
		{
			SetPhysicsProcess(false);
			SignalManager.EmitOnGameOver();
			return false;
		}

		return true;
	}

	private void ApplyHit()
	{
		if (_invincible) return;
		if (!ReduceLives()) return;
		
		GoInvincible();
		SetState(PlayerState.Hurt);
		SignalManager.EmitOnPlayerHit(_lives);
		SoundManager.PlayClip(_soundPlayer, SoundManager.SoundDamage);
	}

	private void GoInvincible()
	{
		_invincibleAnimationPlayer.Play("invincible");
		_invincible = true;
		_invincibleTimer.Start();
	}

	private void OnInvincibleTimerTimeout()
	{
		GD.Print("Invincible timer timeout");
		_invincibleAnimationPlayer.Stop();
		_invincible = false;
	}

	private void OnHurtTimerTimeout()
	{
		SetState(PlayerState.Idle);
	}
	
	private void CalculateState()
	{
		if (_state == PlayerState.Hurt) return;
		
		PlayerState newState;

		if (IsOnFloor())
		{
			if (Velocity.X == 0)
				newState = PlayerState.Idle;
			else
				newState = PlayerState.Run;
		}
		else if (Velocity.Y > 0)
		{
			newState = PlayerState.Fall;
		}
		else
		{
			newState = PlayerState.Jump;
		}
		
		SetState(newState);
	}

	private void SetState(PlayerState newState)
	{
		if (newState == _state) return;
		
		_state = newState;
		switch (_state)
		{
			case PlayerState.Idle:
				_animationPlayer.Play("idle");
				break;
			case PlayerState.Run:
				_animationPlayer.Play("run");
				break;
			case PlayerState.Jump:
				_animationPlayer.Play("jump");
				break;
			case PlayerState.Fall:
				_animationPlayer.Play("fall");
				break;
			case PlayerState.Hurt:
				ApplyHurtJump();
				break;
		}
	}
}
