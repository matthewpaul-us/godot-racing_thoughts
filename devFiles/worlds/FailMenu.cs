using Godot;
using System;

public class FailMenu : TextureRect
{
	public float TimeSeconds;

	private Button _mainMenuButton;
	private Button _restartButton;
	private RichTextLabel _chat;

	public override void _Ready()
	{
		_mainMenuButton = GetNode<Button>("MainMenuButton");
		_restartButton = GetNode<Button>("RestartButton");
		_chat = GetNode<RichTextLabel>("RichTextLabel");

		_mainMenuButton.Connect("pressed", this, nameof(OnMainMenuButtonPressed));
		_restartButton.Connect("pressed", this, nameof(OnRestartButtonPressed));
	}

	public void ShowFail(float timeSeconds)
	{
		TimeSeconds = timeSeconds;

		Show();

		SetChat(timeSeconds);
		AppendChat("John", "Wow, asleep again.");
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
