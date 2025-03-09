using Godot;
using System;
using System.Collections.Generic;

class TargetDistanceTime
{
	public Vector2 PositionTo;
	public float Time;

	public TargetDistanceTime(Vector2 positionFrom, Vector2 positionTo, float speed)
	{
		Time = positionFrom.DistanceTo(positionTo) / speed;
		PositionTo = positionTo;
	}
}


public partial class MovingPlatformAnim : AnimatableBody2D
{
	[Export] private Godot.Collections.Array<Marker2D> _points = new();
	[Export] private float _speed = 150.0f;

	private List<TargetDistanceTime> _targetPoints = new();
	private Tween _tween;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_points.Count < 2) return;

		SignalManager.Instance.OnGameOver += _OnGameOver;
		SignalManager.Instance.OnLevelComplete += _OnGameOver;
		
		GlobalPosition = _points[0].Position;
		Setup();
		RunTween();
	}

	public override void _ExitTree()
	{
		if (_points.Count >= 2)
		{
			SignalManager.Instance.OnGameOver -= _OnGameOver;
			SignalManager.Instance.OnLevelComplete -= _OnGameOver;
		}
		
		_KillTween();
	}

	private void _OnGameOver()
	{
		_KillTween();
	}

	private void _KillTween()
	{
		if (_tween != null)
		{
			_tween.Kill();
		}
	}

	private void Setup()
	{
		for (int i = 0; i < _points.Count - 1; i++)
		{
			_targetPoints.Add(
				new TargetDistanceTime( _points[0].GlobalPosition, _points[i+1].GlobalPosition, _speed));
		}
		_targetPoints.Add(
			new TargetDistanceTime( _points[_points.Count - 1].GlobalPosition, _points[0].GlobalPosition, _speed));
	}

	private void RunTween()
	{
		_tween = GetTree().CreateTween();
		_tween.SetLoops(0);
		foreach (TargetDistanceTime targetPoint in _targetPoints)
		{
			_tween.TweenProperty(this, PropertyName.GlobalPosition.ToString(), targetPoint.PositionTo, targetPoint.Time);
		}

		_tween.TweenInterval(0.02f);
	}
}
