using Godot;
using System;

public partial class Shooter : Node2D
{
	[Export] private float _speed = 50.0f;
	[Export] private float _lifeSpan = 10.0f;
	[Export] private GameObjectType _bulletKey;
	[Export] private float _shootDelay = 0.7f;

	[Export] private AudioStreamPlayer2D _sound;
	[Export] private Timer _shootTimer;

	private bool _canShoot = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shootTimer.WaitTime = _shootDelay;
		_shootTimer.Timeout += OnShootTimerTimeOut;
	}

	public void Shoot(Vector2 direction)
	{
		if (!_canShoot) return;
		
		_canShoot = false;
		SignalManager.EmitOnCreateBullet(GlobalPosition, direction, _speed, _lifeSpan, (int) _bulletKey);
		SoundManager.PlayClip(_sound, SoundManager.SoundLaser);
		_shootTimer.Start();
	}

	private void OnShootTimerTimeOut()
	{
		_canShoot = true;
	}
}
