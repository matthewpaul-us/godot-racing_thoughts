using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class FailMenu : TextureRect
{

	[Export] public List<string> Names;
	[Export] public List<string> Messages;

	public float TimeSeconds;

	private Button _mainMenuButton;
	private Button _restartButton;
	private RichTextLabel _chat;
	private AugmentedRandom _rand;

	private float nextMessageTime;
	private Queue<string> messageQueue;

	public override void _Ready()
	{
		_mainMenuButton = GetNode<Button>("MainMenuButton");
		_restartButton = GetNode<Button>("RestartButton");
		_chat = GetNode<RichTextLabel>("RichTextLabel");
		_rand = Globals.LevelLoader.Random;

		_mainMenuButton.Connect("pressed", this, nameof(OnMainMenuButtonPressed));
		_restartButton.Connect("pressed", this, nameof(OnRestartButtonPressed));

		_rand.Shuffle(Messages);

		messageQueue = new Queue<string>(Messages);

		SetPhysicsProcess(false);
	}

	public override void _PhysicsProcess(float delta)
	{
		nextMessageTime -= delta;

		if(nextMessageTime <= 0 && messageQueue.Any())
		{
			nextMessageTime = (float)_rand.NextDouble() * 3 + 1.5f;
			var message = messageQueue.Dequeue();
			var name = _rand.Random(Names);

			AppendChat(name, message);
		}
	}

	public void ShowFail(float timeSeconds)
	{
		TimeSeconds = timeSeconds;

		Show();

		SetChat(timeSeconds);

		nextMessageTime = (float)_rand.NextDouble() * 3 + 1.5f;
		SetPhysicsProcess(true);
	}

	public void SetChat(float timeSeconds)
	{
		_chat.BbcodeText = $"[i]Inactive for {timeSeconds:.00} seconds[/i]";
	}

	public void AppendChat(string name, string message)
	{
		_chat.AppendBbcode($"\n\n[b]{name}[/b]: {message}");
	}

	public void OnMainMenuButtonPressed()
	{
		Globals.LevelLoader.LoadMenu("res://MainMenu.tscn");
	}
	public void OnRestartButtonPressed()
	{
		Globals.LevelLoader.ReloadLevel();
	}
}
