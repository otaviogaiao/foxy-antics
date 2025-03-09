using Godot;
using System;

[GlobalClass]
public partial class EnemyBase : CharacterBody2D
{
	[Export] protected int _points = 1;
	[Export] protected float _speed = 30.0f;
	[Export] protected float _yFallOff = 100.0f;

	[Export] protected VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;
	[Export] protected AnimatedSprite2D _animatedSprite2D;
	[Export] protected Area2D _hitBox;
	
	protected float _gravity = 800.0f;
	protected Player _playerRef;

	public override void _Ready()
	{
		_playerRef = (Player) GetTree().GetFirstNodeInGroup(Player.GroupName);
		_visibleOnScreenNotifier2D.ScreenEntered += OnScreenEntered;
		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExited;
		_hitBox.AreaEntered += OnHitBoxAreaEntered;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		FallenOff();
	}


	protected virtual void OnScreenEntered()
	{
	}

	protected virtual void OnScreenExited()
	{
	}

	protected void OnHitBoxAreaEntered(Area2D area)
	{
		GD.Print("OnHitBoxAreaEntered");
		Die();
	}
	
	private void Die()
	{
		SignalManager.EmitOnEnemyHit(_points, GlobalPosition);
		SignalManager.EmitOnCreateObject(GlobalPosition, (int) GameObjectType.Explosion);
		SignalManager.EmitOnCreateObject(GlobalPosition, (int) GameObjectType.Pickup);
		SetPhysicsProcess(false);
		QueueFree();
	}
	
	private void FallenOff()
	{
		if (GlobalPosition.Y > _yFallOff)
		{
			QueueFree();
		}
	}
}
