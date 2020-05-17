using Godot;
using System;

public class Person : Area2D
{
	public Sprite FocusIcon;

	public Thought Thought;

	[Signal] public delegate void ThoughtClicked(Person person, ThoughtPart part);
	public override void _Ready()
	{
		Thought = GetNode<Thought>("Thought");
		FocusIcon = GetNode<Sprite>("FocusIcon");
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
			FocusIcon.Show();
		}
		else
		{
			Thought.SetFreeze(false);
			FocusIcon.Hide();
		}
	}

	public void ShowThought()
	{
		Thought.ShowThought(4);
	}

	public bool HasThought(ThoughtPart part)
	{
		return Thought.HasThought(part);
	}
}
