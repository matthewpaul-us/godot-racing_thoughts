using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D
{
	private Random _rand;

	private List<Person> people;

	public override void _Ready()
	{
		var seed = "Fair Time Grows";

		_rand = RandomSingleton.GetInstance(seed);

		people = GetChildren()
			.Cast<Node>()
			.Where(c => c is Person)
			.Cast<Person>()
			.ToList()
			;

		var timer = GetNode<Timer>("Timer");

		timer.Connect("timeout",this, nameof(OnTimerTimeout));
	}

	public void OnTimerTimeout()
	{
		var randomPerson = people[_rand.Next(people.Count())];

		randomPerson.ShowThought();
	}


}
