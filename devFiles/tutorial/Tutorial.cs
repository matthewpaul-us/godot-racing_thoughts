using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class Tutorial : Node2D
{
	private AugmentedRandom _rand;

	private Person _firstPerson;
	private Person focusPerson;
	private Person targetPerson;
	private List<Person> _people;
	private Camera2D _camera;
	private PersonSpawner _spawner;
	private YSort _sort;
	private TargetCamera _targetCam;

	private Node2D _josephSpawn;
	private Node2D _spawn1;
	private Node2D _spawn2;
	private Node2D _spawn3;
	private Node2D _finalSpawn;

	private AnimationPlayer _message1Anim;
	private AnimationPlayer _message2Anim;
	private AnimationPlayer _message3Anim;

	private Person _tut1Person;
	private Person _tut2Person;
	private Person _middlePerson;
	private Label _hoverMessage;
	private Label _clickMessage;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera");
		_spawner = GetNode<PersonSpawner>("PersonSpawner");
		_sort = GetNode<YSort>("YSort");
		_targetCam = GetNode<TargetCamera>("TargetCamera");
		_josephSpawn = GetNode<Node2D>("JosephSpawn");
		_finalSpawn = GetNode<Node2D>("FinalSpawn");
		_spawn1 = GetNode<Node2D>("personSpawn");
		_spawn2 = GetNode<Node2D>("personSpawn2");
		_spawn3 = GetNode<Node2D>("personSpawn3");
		_message1Anim = GetNode<AnimationPlayer>("TutorialMessage/AnimationPlayer");
		_message2Anim = GetNode<AnimationPlayer>("TutorialMessage2/AnimationPlayer");
		_message3Anim = GetNode<AnimationPlayer>("TutorialMessage3/AnimationPlayer");
		_hoverMessage = GetNode<Label>("HoverMessage");
		_clickMessage = GetNode<Label>("ClickMessage");

		_rand = RandomSingleton.GetInstance();
		_people = new List<Person>();

		_spawner.SortLayer = _sort;



		var joseph = _spawner.SpawnAt(_josephSpawn.Position, true);
		var furthestPerson = _spawner.SpawnAt(_finalSpawn.Position);

		_tut1Person = _spawner.SpawnAt(_spawn1.Position);
		_middlePerson = _spawner.SpawnAt(_spawn2.Position);
		_tut2Person = _spawner.SpawnAt(_spawn3.Position);

		_people.Add(joseph);
		_people.Add(furthestPerson);

		_people.Add(_tut1Person);
		_people.Add(_middlePerson);
		_people.Add(_tut2Person);

		//_tut1Person.Connect(nameof(Person.Hovered), this, nameof(OnPersonHovered));

		SetFocusPerson(joseph);
		SetTargetPerson(furthestPerson);

		joseph.ForceThoughtParts("home", "home", "home");
		joseph.ShowThought();

		_tut1Person.ForceThoughtParts("home", "home", "home");
		_middlePerson.ForceThoughtParts("dog", "money", "home");
		_tut2Person.ForceThoughtParts("money", "home", "clock");
		furthestPerson.ForceThoughtParts("clock", "money-bill", "money-bill");

		foreach (var person in _people)
		{
			person.Connect(nameof(Person.ThoughtClicked), this, nameof(OnPersonThoughtClicked));
			person.Brain.InitialState = "frozen";
		}

		_firstPerson = joseph;


		_message1Anim.Play("swoop_in");
		_hoverMessage.GetNode<AnimationPlayer>("AnimationPlayer").Play("flash");
	}

	public void OnPersonHovered()
	{
		_hoverMessage.Hide();
	}

	public void PlayMessage(AnimationPlayer player)
	{
		player.Play("swoop_in");
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
			part.Shake();
			return;
		}

		part.Roll();
		SetFocusPerson(person);
		person.PlayWhoosh();
		person.SetShine(true);

		_targetCam.Show();

		if(person == _tut1Person)
		{
			PlayMessage(_message2Anim);
			_clickMessage.Hide();
			_hoverMessage.Hide();
		}

		if(person == _tut2Person)
		{
			PlayMessage(_message3Anim);
		}

		if(person == targetPerson)
		{
			PlayWin();
		}
	}

	private async void PlayWin()
	{
		Globals.HasPlayedTutorial = true;

		await ToSignal(GetTree().CreateTimer(1.5f), "timeout");

		Globals.LevelLoader.LoadLevel("res://worlds/World.tscn");
	}
}
