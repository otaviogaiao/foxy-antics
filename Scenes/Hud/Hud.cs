using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hud : Control
{
	private const string MOVEABLES = "Moveables";
	private List<TextureRect> _hearts;

	[Export] private Label _scoreLabel;
	[Export] private HBoxContainer _hbHearts;
	[Export] private VBoxContainer _vbLevelComplete;
	[Export] private VBoxContainer _vbGameOver;
	[Export] private AudioStreamPlayer _sound;
	[Export] private Timer _continueTimer;
	[Export] private ColorRect _colorRect;

	private bool _canContinue = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hearts = _hbHearts.GetChildren().OfType<TextureRect>().ToList();
		OnScoreUpdated();
		ConnectSignals();
	}

	public override void _ExitTree()
	{
		DisconnectSignals();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_canContinue && Input.IsActionJustPressed("shoot"))
		{
			if (_vbLevelComplete.Visible)
			{
				GameManager.LoadNextLevelScene();
			}
			else
			{
				GameManager.LoadMainScene();
			}
		}
	}

	private void UpdateHud(int lives)
	{
		for (int i = 0; i < _hearts.Count; i++)
		{
			_hearts[i].Visible = lives > i;
		}
	}

	private void OnScoreUpdated()
	{
		_scoreLabel.Text = ScoreManager.Score.ToString("D5");
	}

	private void ConnectSignals()
	{
		SignalManager.Instance.OnLevelStart += UpdateHud;
		SignalManager.Instance.OnPlayerHit += UpdateHud;
		SignalManager.Instance.OnScoreUpdated += OnScoreUpdated;
		SignalManager.Instance.OnGameOver += OnGameOver;
		SignalManager.Instance.OnLevelComplete += OnLevelComplete;
		_continueTimer.Timeout += OnContinueTimer;
	}

	private void DisconnectSignals()
	{
		SignalManager.Instance.OnLevelStart -= UpdateHud;
		SignalManager.Instance.OnPlayerHit -= UpdateHud;
		SignalManager.Instance.OnScoreUpdated -= OnScoreUpdated;
		SignalManager.Instance.OnGameOver -= OnGameOver;
		SignalManager.Instance.OnLevelComplete -= OnLevelComplete;
	}

	private void StopAllMoveables()
	{
		List<Node2D> moveablesNodes = GetTree().GetNodesInGroup(MOVEABLES).OfType<Node2D>().ToList();
		foreach (Node2D node in moveablesNodes)
		{
			node.SetProcess(false);
			node.SetPhysicsProcess(false);
		}
	}
	
	private void ShowHud(bool gameOver)
	{
		if (gameOver)
		{
			_vbGameOver.Show();
			_sound.Play();
		}
		else
		{
			_vbLevelComplete.Show();
		}
		_continueTimer.Start();
		_colorRect.Show();
		StopAllMoveables();
	}

	private void OnGameOver()
	{
		ShowHud(true);
	}

	private void OnLevelComplete()
	{
		ShowHud(false);
	}

	private void OnContinueTimer()
	{
		_canContinue = true;
	}
}
