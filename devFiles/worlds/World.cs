using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D
{
	private Random _rand;

	private Person _firstPerson;
	private Person focusPerson;
	private Person targetPerson;
	private List<Person> _people;
	private Camera2D _camera;
	private PersonSpawner _spawner;
	private WorldGUI _gui;
	private YSort _sort;
	private TargetCamera _targetCam;
	private Infection _infection;
	private LevelStartCutscene _cutscene;
	private int _connectionsMade;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera");
		_spawner = GetNode<PersonSpawner>("PersonSpawner");
		_gui = GetNode<WorldGUI>("WorldGUI");
		_sort = GetNode<YSort>("YSort");
		_targetCam = GetNode<TargetCamera>("TargetCamera");
		_infection = GetNode<Infection>("Infection");
		_cutscene = GetNode<LevelStartCutscene>("LevelStartCutscene");

		_spawner.SpawnArea = GetNode<TextureRect>("SpawnerRect");
		_spawner.SortLayer = _sort;

		_rand = RandomSingleton.GetInstance();

		_spawner.Spawn();

		_people = _sort.GetChildren()
			.Cast<Node>()
			.Where(c => c is Person)
			.Cast<Person>()
			.ToList()
			;

		_infection.HealthyPeople.AddRange(_people);

		Person minXPerson = null, maxXPerson = null;
		foreach (var person in _people)
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

			person.Connect(nameof(Person.Infected), this, nameof(OnInfectionInfected));
		}

		// Have to defer to give the FSM a chance to catch up on first execution
		CallDeferred(nameof(SetFocusPerson), minXPerson);
		CallDeferred(nameof(SetTargetPerson), maxXPerson);

		_firstPerson = minXPerson;

		SetFocusPerson(minXPerson);
		SetTargetPerson(maxXPerson);

		PlayStartCutscene();
	}

	private void PlayStartCutscene()
	{
		_cutscene.SetupCamera(GetNode<TextureRect>("Background").RectSize / 2f);
		_cutscene.UsePeople(focusPerson, targetPerson);
	}

	private void SetTargetPerson(Person person)
	{
		targetPerson = person;

		person.IsTarget = true;

		_targetCam.TargetPerson = person;
		person.SetShine(true);
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

		SetFocusPerson(person);
		person.PlayWhoosh();
		person.SetShine(true);

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

	public void PlayLose()
	{
		_infection.IsRunning = false;
		_gui.ShowFailMenu();
	}

	public override void _PhysicsProcess(float delta)
	{
		_infection.InfectionTimerPercent = _gui.InfectionTimeLeftPercent;
	}

	public void SetFocusPerson(Person person)
	{
		// Make sure to unset the old
		focusPerson?.SetFocus(false);
		focusPerson?.SetShine(false);

		focusPerson = person;
		person.SetFocus(true);
		person.SetShine(true);

		_camera.Position = person.Position;

		_targetCam.FocusPerson = person;
	}
	private void OnWorldGuiTimeRanOut()
	{
		_infection.StartInfection(_firstPerson);
	}
	private void OnInfectionInfected(Person infectedPerson)
	{
		if(infectedPerson == focusPerson)
		{
			PlayLose();
		}

	}

	private void OnLevelStartCutsceneCutsceneFinished()
	{
		PlayStart();
	}

	private void PlayStart()
	{
		_camera.Current = true;
		focusPerson.ShowThought();
		_gui.StartTimer(30);
	}
}

