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
	private Camera2D _camera;
	private PersonSpawner _spawner;
	private WorldGUI _gui;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera");
		_spawner = GetNode<PersonSpawner>("PersonSpawner");
		_gui = GetNode<WorldGUI>("WorldGUI");

		_spawner.SpawnArea = GetNode<TextureRect>("SpawnerRect");

		var seed = "Fair Time Grows";

		_rand = RandomSingleton.GetInstance(seed);

		_spawner.Spawn();

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

		SetFocusPerson(person);
	}

	public void OnTimerTimeout()
	{
		var randomPerson = people[_rand.Next(people.Count())];

		if (focusPerson == null)
		{
			SetFocusPerson(randomPerson);
			randomPerson.ShowThought();

			_gui.StartTimer(30);
		}
	}

	public void SetFocusPerson(Person person)
	{
		// Make sure to unset the old
		focusPerson?.SetFocus(false);

		focusPerson = person;
		person.SetFocus(true);
		_camera.Position = person.Position;
	}
}
