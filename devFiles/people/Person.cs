using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;

public class Person : KinematicBody2D
{
	[Signal] public delegate void Infected(Person person);

	public Sprite FocusIcon;

	[Export] public Color NormalColor;
	[Export] public Color ShiningColor;
	[Export] public Color InfectedColor;
	[Export] public Color SelectedColor;

	public Thought Thought;
	public Vector2 Velocity = Vector2.Zero;
	private AnimationPlayer _anim;
	private PersonStateMachine _brain;
	private bool _hasFocus;
	private bool _isTarget;
	private Random _rand;
	private Sprite _sprite;
	private Timer _timer;
	private Tween _tween;
	private AudioStreamPlayer2D _whoosh;
	[Signal] public delegate void ThoughtClicked(Person person, ThoughtPart part);

	internal Texture GetTexture()
	{
		return _sprite.Texture;
	}

	public bool HasFocus { get { return _hasFocus; } set { SetFocus(value); } }
	public bool IsTarget
	{
		get { return _isTarget; }
		set { SetIsTarget(value); }
	}

	public override void _PhysicsProcess(float delta)
	{
		MoveAndCollide(Velocity);
	}

	private List<string> InfectedThoughts;
	internal void Infect(List<string> parts)
	{
		InfectedThoughts = parts;
		// Have to ensure infected can happen off-screen
		if(_brain.IsPhysicsProcessing())
		{
			_brain.SetState("get_infected");
		}
		else
		{
			_sprite.Modulate = InfectedColor;
			Thought.SetThoughtParts(InfectedThoughts);
		}
	}

	private void InfectInternal()
	{
		_sprite.Modulate = InfectedColor;
		Thought.SetThoughtParts(InfectedThoughts);
		EmitSignal(nameof(Infected), this);
	}

	public override void _Ready()
	{
		_rand = RandomSingleton.GetInstance();

		Thought = GetNode<Thought>("Thought");
		FocusIcon = GetNode<Sprite>("FocusIcon");
		_timer = GetNode<Timer>("Timer");
		_sprite = GetNode<Sprite>("Sprite");
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		_brain = GetNode<PersonStateMachine>("Brain");
		_tween = GetNode<Tween>("Tween");
		_whoosh = GetNode<AudioStreamPlayer2D>("WhooshSound");

		_timer.Connect("timeout", this, nameof(OnTimerTimeout));
		_timer.WaitTime = (float)(_rand.NextDouble() * 2.0 + 2.0);
		_timer.Start();

		Thought.Randomize();
	}

	public bool HasThought(ThoughtPart part)
	{
		return Thought.HasThought(part);
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

	public void OnTimerTimeout()
	{
		Thought.Randomize();
	}

	public void PlayAnimation(string animationName)
	{
		if (!string.IsNullOrWhiteSpace(animationName))
		{
			// I had to use safe access here for some reason because at the beginning _anim was null
			_anim?.Play(animationName);
		}
		else
		{
			_anim?.Stop();
		}
	}

	public void SetFocus(bool isFocused)
	{
		if (isFocused)
		{
			Thought.SetFreeze(true);
			Thought.ZIndex += 10;
			_brain.SetState("picked");
		}
		else
		{
			Thought.SetFreeze(false);
			Thought.ZIndex -= 10;
			_brain.SetState("wait");
		}

		_hasFocus = isFocused;
	}

	public void SetShine(bool isShining)
	{
		if (isShining)
		{
			_tween.InterpolateProperty(_sprite, "modulate", NormalColor, SelectedColor,
				1, Tween.TransitionType.Circ, Tween.EaseType.Out);
		}
		else
		{
			_tween.InterpolateProperty(_sprite, "modulate", SelectedColor, ShiningColor,
				0.5f, Tween.TransitionType.Circ, Tween.EaseType.In);
		}

		_tween.Start();
	}

	public void PlayWhoosh()
	{
		_whoosh.Play();
	}

	public void SetTarget(bool isTarget)
	{

	}

	public void ShowThought()
	{
		Thought.ShowThought(4);
	}

	internal void SetColor(Color color)
	{
		_sprite.Modulate = color;
	}

	internal void SetTexture(Texture texture)
	{
		_sprite.Texture = texture;
	}

	private void OnPersonMouseEntered()
	{
		if (!Thought.IsThoughtVisible)
		{
			ShowThought();
		}
	}

	private void SetIsTarget(bool value)
	{
		_isTarget = value;

		Thought.SetFreeze(true);
		Thought.ZIndex += 10;

		SetShine(true);
	}
	private void OnVisibilityEnabler2DScreenEntered()
	{
		_brain.SetPhysicsProcess(true);
	}


	private void OnVisibilityEnabler2DScreenExited()
	{
		_brain.SetPhysicsProcess(false);
	}

	public bool IsPlayingAnimation()
	{
		return _anim.IsPlaying();
	}
}
