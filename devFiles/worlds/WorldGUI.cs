using Godot;
using System;
using System.Runtime.CompilerServices;

public class WorldGUI : CanvasLayer
{
	private Control _timeLeftControl;
	private Label _timeLeftLabel;
	private TextureProgress _progressBar;
	private Area2D _hoverCheckArea;
	private EndWorldMenu _endMenu;

	private float _totalTimeAvailable;
	private float _timeLeft;
	private bool isTimerRunning;

	public override void _Ready()
	{
		_timeLeftControl = GetNode<Control>("TimeLeftControl");
		_hoverCheckArea = GetNode<Area2D>("TimeLeftControl/Area2D");
		_timeLeftLabel = GetNode<Label>("TimeLeftControl/TimeLeftLabel");
		_progressBar = GetNode<TextureProgress>("TimeLeftControl/ProgressBar");
		_endMenu = GetNode<EndWorldMenu>("EndWorldMenu");

		_hoverCheckArea.Connect("mouse_entered", this, nameof(OnTimeLeftControlMouseEnter));
		_hoverCheckArea.Connect("mouse_exited", this, nameof(OnTimeLeftControlMouseExit));
	}

	public override void _PhysicsProcess(float delta)
	{
		if(isTimerRunning)
		{
			_timeLeft -= delta;

			_timeLeftLabel.Text = _timeLeft.ToString("0.00");
			_progressBar.Value = _timeLeft;
		}
	}

	public void StartTimer(float timeSeconds)
	{
		_timeLeftLabel.Text = timeSeconds.ToString("0.00");
		_progressBar.MaxValue = timeSeconds;
		_progressBar.Value = _timeLeft;

		_totalTimeAvailable = timeSeconds;
		_timeLeft = timeSeconds;
		isTimerRunning = true;
	}

	public void StopTimer()
	{
		isTimerRunning = false;
	}

	public void ShowEndWorldMenu(string seed, int connections)
	{
		_endMenu.SetSeed(seed);
		_endMenu.SetTimeScore(_totalTimeAvailable - _timeLeft);
		_endMenu.SetConnectionScore(connections);

		_endMenu.Show();
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
