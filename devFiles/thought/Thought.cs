using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Thought : Node2D
{
	private bool _isFrozen;

	private Sprite _shadow;

	private AnimationPlayer _anim;

	public bool IsFrozen
	{
		get { return _isFrozen; }
		set { SetFreeze(value); }
	}

	public bool IsThoughtVisible { get; set; }

	public bool MyProperty { get; set; }
	public List<ThoughtPart> parts;

	private Timer _timer;
	public override void _Ready()
	{
		_shadow = GetNode<Sprite>("Shadow");
		_timer = GetNode<Timer>("Timer");
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");

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
			Scale = Vector2.One * 1.5f;
			_shadow.Show();
		}
		else
		{
			_timer.Paused = false;
			Scale = Vector2.One;
			_shadow.Hide();
		}

		_isFrozen = isFrozen;
	}

	public void SetThoughtVisible(bool isVisible, float durationSeconds)
	{
		if (isVisible)
		{
			_timer.WaitTime = durationSeconds;
			_timer.Start();

			if(!IsFrozen)
			{
				//Randomize();
			}

			Show();
			_anim.Play("show");
		}
		else
		{
			_anim.Play("hide");
			_timer.Stop();
		}

		IsThoughtVisible = isVisible;
	}

	public void Randomize()
	{
		if(!IsThoughtVisible)
		{
			foreach (var part in parts)
			{
				part.Randomize();
			}
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
