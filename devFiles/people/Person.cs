using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;

public class Person : Area2D
{
	public Sprite FocusIcon;

	public Thought Thought;

	private bool _hasFocus;
	public bool HasFocus { get { return _hasFocus; } set { SetFocus(value); } }

	private bool _isTarget;

	private Sprite _sprite;

	public bool IsTarget
	{
		get { return _isTarget; }
		set { SetIsTarget(value); }
	}

	private void SetIsTarget(bool value)
	{
		_isTarget = value;
		FocusIcon.Visible = _isTarget;

		Thought.SetFreeze(true);
		Thought.ZIndex += 10;
		FocusIcon.Show();
	}

	[Signal] public delegate void ThoughtClicked(Person person, ThoughtPart part);

	private Timer _timer;
	private Random _rand;

	public override void _Ready()
	{
		_rand = RandomSingleton.GetInstance();

		Thought = GetNode<Thought>("Thought");
		FocusIcon = GetNode<Sprite>("FocusIcon");
		_timer = GetNode<Timer>("Timer");
		_sprite = GetNode<Sprite>("Sprite");

		_timer.Connect("timeout", this, nameof(OnTimerTimeout));
		_timer.WaitTime = (float)(_rand.NextDouble() * 2.0 + 2.0);
		_timer.Start();
	}

	internal void SetTexture(Texture texture)
	{
		_sprite.Texture = texture;
	}

	internal void SetColor(Color color)
	{
		_sprite.Modulate = color;
	}

	public void HideThought()
	{
		Thought.HideThought();
	}

	public void OnThoughtPartClicked(ThoughtPart part)
	{
		GD.Print($"My {part.Part} thought was clicked!");
		EmitSignal(nameof(ThoughtClicked), this, part);
	}

	public void SetFocus(bool isFocused)
	{
		if (isFocused)
		{
			Thought.SetFreeze(true);
			Thought.ZIndex += 10;
			FocusIcon.Show();
		}
		else
		{
			Thought.SetFreeze(false);
			Thought.ZIndex -= 10;
			FocusIcon.Hide();
		}

		_hasFocus = isFocused;
	}

	public void ShowThought()
	{
		Thought.ShowThought(4);
	}

	public bool HasThought(ThoughtPart part)
	{
		return Thought.HasThought(part);
	}

	public void OnTimerTimeout()
	{
		ShowThought();
	}

	public void SetTarget(bool isTarget)
	{

	}
}
