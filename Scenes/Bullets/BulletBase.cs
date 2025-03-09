using Godot;
using System;

public partial class BulletBase : Area2D
{
	private Vector2 _direction = Vector2.Right;
	private double _lifeSpan = 20.0f;
	private double _lifeTime = 0.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckExpired(delta);
		Position += _direction * (float) delta;
	}
	
	public void Setup(Vector2 dir, float lifeSpan, float speed)
	{
		_direction = dir.Normalized() * speed;
		_lifeSpan = lifeSpan;
	}

	private void CheckExpired(double delta)
	{
		_lifeTime += delta;
		if (_lifeTime >= _lifeSpan)
		{
			SetProcess(false);
			QueueFree();
		}
	}

	protected virtual void OnAreaEntered(Area2D area)
	{
		QueueFree();
	}
}
