using Godot;
using System;

public partial class Explosion : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}
}
