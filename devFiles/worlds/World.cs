using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D
{
	private Random _rand;

	private Person focusPerson;
	private Person targetPerson;
	private List<Person> people;
	private Camera2D _camera;
	private PersonSpawner _spawner;
	private WorldGUI _gui;
	private YSort _sort;
	private TargetCamera _targetCam;

	private int _connectionsMade;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera");
		_spawner = GetNode<PersonSpawner>("PersonSpawner");
		_gui = GetNode<WorldGUI>("WorldGUI");
		_sort = GetNode<YSort>("YSort");
		_targetCam = GetNode<TargetCamera>("TargetCamera");

		_spawner.SpawnArea = GetNode<TextureRect>("SpawnerRect");
		_spawner.SortLayer = _sort;

		var seed = "Fair Time Grows";

		_rand = RandomSingleton.GetInstance(seed);

		_spawner.Spawn();

		people = _sort.GetChildren()
			.Cast<Node>()
			.Where(c => c is Person)
			.Cast<Person>()
			.ToList()
			;

		Person minXPerson = null, maxXPerson = null;
		foreach (var person in people)
		{
			person.Connect(nameof(Person.ThoughtClicked), this, nameof(OnPersonThoughtClicked));

			// Find the left most person
			if(minXPerson == null || person.Position.x < minXPerson.Position.x)
			{
				minXPerson = person;
			}

			// Find the right most person
			if(maxXPerson == null || person.Position.x > maxXPerson.Position.x)
			{
				maxXPerson = person;
			}
		}
		SetFocusPerson(minXPerson);
		SetTargetPerson(maxXPerson);
		minXPerson.ShowThought();
		_gui.StartTimer(30);
	}

	private void SetTargetPerson(Person person)
	{
		targetPerson = person;

		person.IsTarget = true;

		_targetCam.TargetPerson = person;
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

		_connectionsMade++;

		if(person == targetPerson)
		{
			PlayWin();
		}
	}

	public void PlayWin()
	{
		_gui.StopTimer();
		_gui.ShowEndWorldMenu(Globals.LevelLoader.Random.Seed, _connectionsMade);
	}

	public void SetFocusPerson(Person person)
	{
		// Make sure to unset the old
		focusPerson?.SetFocus(false);

		focusPerson = person;
		person.SetFocus(true);
		_camera.Position = person.Position;

		_targetCam.FocusPerson = person;
	}
}
