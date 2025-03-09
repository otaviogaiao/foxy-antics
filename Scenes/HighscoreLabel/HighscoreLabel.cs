using Godot;
using System;

public partial class HighscoreLabel : VBoxContainer
{
	[Export] private Label _scoreLabel;
	[Export] private Label _dateLabel;

	public void SetHighScoreText(GameScore score)
	{
		_scoreLabel.Text = score.Score.ToString();
		_dateLabel.Text = score.DateAchieved.ToShortDateString();
	}
}
