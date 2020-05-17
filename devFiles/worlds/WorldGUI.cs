using Godot;
using System;
using System.Runtime.CompilerServices;

public class WorldGUI : CanvasLayer
{
	private Control _timeLeftControl;
	private Label _timeLeftLabel;
	private TextureProgress _progressBar;
	private Area2D _hoverCheckArea;

	private float timeLeft;
	private bool isTimerRunning;

	public override void _Ready()
	{
		_timeLeftControl = GetNode<Control>("TimeLeftControl");
		_hoverCheckArea = GetNode<Area2D>("TimeLeftControl/Area2D");
		_timeLeftLabel = GetNode<Label>("TimeLeftControl/TimeLeftLabel");
		_progressBar = GetNode<TextureProgress>("TimeLeftControl/ProgressBar");

		_hoverCheckArea.Connect("mouse_entered", this, nameof(OnTimeLeftControlMouseEnter));
		_hoverCheckArea.Connect("mouse_exited", this, nameof(OnTimeLeftControlMouseExit));
	}

	public override void _PhysicsProcess(float delta)
	{
		if(isTimerRunning)
		{
			timeLeft -= delta;

			_timeLeftLabel.Text = timeLeft.ToString("0.00");
			_progressBar.Value = timeLeft;
		}
	}

	public void StartTimer(float timeSeconds)
	{
		_timeLeftLabel.Text = timeSeconds.ToString("0.00");
		_progressBar.MaxValue = timeSeconds;
		_progressBar.Value = timeLeft;
		timeLeft = timeSeconds;
		isTimerRunning = true;
	}

	public void OnTimeLeftControlMouseEnter()
	{
		var color = _timeLeftControl.Modulate;
		color.a = 0.6f;
		_timeLeftControl.Modulate = color;
	}

	public void OnTimeLeftControlMouseExit()
	{
		var color = _timeLeftControl.Modulate;
		color.a = 1f;
		_timeLeftControl.Modulate = color;
	}
}
