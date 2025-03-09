using Godot;
using System;

public partial class Checkpoint : Area2D
{
	[Export] private AnimationTree _animationTree;
	[Export] private AudioStreamPlayer2D _audio;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalManager.Instance.OnBossKilled += OnBossDied;
		AreaEntered += OnAreaEntered;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnBossKilled -= OnBossDied;
	}

	private void OnBossDied(int _)
	{
		SetDeferred(Area2D.PropertyName.Monitoring, true);
		_animationTree.Set("parameters/conditions/on_trigger", true);
	}

	private void OnAreaEntered(Area2D _)
	{
		SignalManager.EmitOnLevelComplete();
		SoundManager.PlayClip(_audio,SoundManager.SoundWin);
	}
}
