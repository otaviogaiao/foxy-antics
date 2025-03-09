using Godot;
using System;

public partial class FruitPickUp : Area2D
{
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private Timer _lifeTimer;
	[Export] private float _jump = -80.0f;
	[Export] private int _points = 2;

	private float _startY;
	private float _speedY;
	private bool _stopped = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_speedY = _jump;
		_startY = Position.Y;
		_lifeTimer.Timeout += OnLifeTimerTimeout;
		AreaEntered += OnAreaEntered;
		PlayRandomAnimation();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_stopped) return;
		
		Position += new Vector2(0, _speedY * (float) delta);
		_speedY += Gravity * (float)delta;

		if (Position.Y > _startY)
		{
			_stopped = true;
		}
	}
	
	private void PlayRandomAnimation()
	{
		var animationNames = _animatedSprite2D.SpriteFrames.GetAnimationNames();

		if (animationNames.Length > 0)
		{
			string randomAnimationName = animationNames[new Random().Next(animationNames.Length)];
			_animatedSprite2D.Play(randomAnimationName);
		}
	}

	private void OnLifeTimerTimeout()
	{
		QueueFree();
	}

	private void OnAreaEntered(Area2D area)
	{
		SignalManager.EmitOnPickupHit(_points);
		QueueFree();
	}
}
