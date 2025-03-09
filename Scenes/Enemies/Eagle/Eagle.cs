using Godot;
using System;

public partial class Eagle : EnemyBase
{
	[Export] private Timer _directionTimer;
	[Export] private Shooter _shooter;
	[Export] private RayCast2D _playerDetect;

	private readonly Vector2 _flySpeed = new (35, 15);
	private Vector2 _flyDirection = Vector2.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_directionTimer.Timeout += OnDirectionTimerTimeout;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Velocity = _flyDirection;
		MoveAndSlide();
		Shoot();
	}

	private void SetDirectionAndFlip()
	{
		int xDirection = Math.Sign(_playerRef.GlobalPosition.X - GlobalPosition.X);
		_animatedSprite2D.FlipH = xDirection > 0;
		_flyDirection = new Vector2(xDirection, 1) * _flySpeed;
	}

	private void FlyToPlayer()
	{
		SetDirectionAndFlip();
		_directionTimer.Start();
	}

	private void OnDirectionTimerTimeout()
	{
		FlyToPlayer();
	}

	private void Shoot()
	{
		if (_playerDetect.IsColliding())
		{
			_shooter.Shoot(GlobalPosition.DirectionTo(_playerRef.GlobalPosition));
		}
	}

	protected override void OnScreenEntered()
	{
		_animatedSprite2D.Play("fly");
		FlyToPlayer();
	}
}
