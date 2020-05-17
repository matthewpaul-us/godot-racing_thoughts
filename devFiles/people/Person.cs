using Godot;
using System;

public class Person : Area2D
{
	public Thought Thought;

	public override void _Ready()
	{
		Thought = GetNode<Thought>("Thought");
	}

	public void ShowThought()
	{
		Thought.ShowThought(4);
	}

	public void HideThought()
	{
		Thought.HideThought();
	}
}
