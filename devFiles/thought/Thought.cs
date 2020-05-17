using Godot;
using System;

public class Thought : Node2D
{
	private Timer _timer;
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		HideThought();

		_timer.Connect("timeout", this, nameof(HideThought));
	}

	public void SetThoughtVisible(bool isVisible, float durationSeconds)
	{
		if (isVisible)
		{
			Show();
			_timer.WaitTime = durationSeconds;
			_timer.Start();
		}
		else
		{
			Hide();
			_timer.Stop();
		}
	}

	internal void HideThought()
	{
		SetThoughtVisible(false, 0);
	}

	internal void ShowThought(float durationSeconds)
	{
		SetThoughtVisible(true, durationSeconds);
	}
}
