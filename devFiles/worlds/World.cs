using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D
{
	private Random _rand;

	private Person focusPerson;
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

		timer.Connect("timeout", this, nameof(OnTimerTimeout));

		foreach (var person in people)
		{
			person.Connect(nameof(Person.ThoughtClicked), this, nameof(OnPersonThoughtClicked));
		}
	}

	public void OnPersonThoughtClicked(Person person, ThoughtPart part)
	{
		// We don't want to transition to the same person
		if(person == focusPerson)
		{
			return;
		}

		GD.Print($"{person.Name}'s {part.Part} was clicked!");
		// He doesn't have the same thought
		if(!focusPerson.HasThought(part))
		{
			return;
		}

		GD.Print($"Transition to {person.Name}");
	}

	public void OnTimerTimeout()
	{
		var randomPerson = people[_rand.Next(people.Count())];

		if (focusPerson == null)
		{
			SetFocusPerson(randomPerson);
			randomPerson.ShowThought();
		}

		if (randomPerson != focusPerson)
		{
			randomPerson.ShowThought();
		}
	}

	public void SetFocusPerson(Person person)
	{
		focusPerson = person;
		person.SetFocus(true);
	}
}
