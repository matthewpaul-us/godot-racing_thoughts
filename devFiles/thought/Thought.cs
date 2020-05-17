using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Thought : Node2D
{
	public List<ThoughtPart> parts;

	private Timer _timer;
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		HideThought();

		_timer.Connect("timeout", this, nameof(HideThought));

		parts = GetChildren()
			.Cast<Node>()
			.Where(n => n.Name.Contains("Thought"))
			.Select(n => n.GetChild<ThoughtPart>(0))
			.ToList()
			;

		var person = GetOwner<Person>();
		foreach (var part in parts)
		{
			part.Connect(nameof(ThoughtPart.Clicked), person, nameof(person.OnThoughtPartClicked));
		}
	}

	public void SetFreeze(bool isFrozen)
	{
		if(isFrozen)
		{
			_timer.Paused = true;
		}
		else
		{
			_timer.Paused = false;
		}

	}

	public void SetThoughtVisible(bool isVisible, float durationSeconds)
	{
		if (isVisible)
		{
			_timer.WaitTime = durationSeconds;
			_timer.Start();

			foreach(var part in parts)
			{
				part.Randomize();
			}

			Show();
		}
		else
		{
			Hide();
			_timer.Stop();
		}
	}

	public bool HasThought(ThoughtPart part)
	{
		return parts.Any(p => p.Part == part.Part);
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
