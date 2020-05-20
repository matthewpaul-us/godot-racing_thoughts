using Godot;
using System;

public class EndWorldMenu : CanvasLayer
{
	private LevelLoader _loader;
	private Label _seedLabel;

	private Label _youTimeLabel;
	private Label _parTimeLabel;

	private Label _youHopsLabel;
	private Label _parHopsLabel;

	public override void _Ready()
	{
		_youTimeLabel = GetNode<Label>("TextureRect/MarginContainer/VBoxContainer/TimeStatPanel/YouTimeLabel");
		_parTimeLabel = GetNode<Label>("TextureRect/MarginContainer/VBoxContainer/TimeStatPanel/ParTimeLabel");

		_youHopsLabel = GetNode<Label>("TextureRect/MarginContainer/VBoxContainer/HopsStatPanel/YouHopsLabel");
		_parHopsLabel = GetNode<Label>("TextureRect/MarginContainer/VBoxContainer/HopsStatPanel/ParHopsLabel");

		_seedLabel = GetNode<Label>("TextureRect/MarginContainer/VBoxContainer/SeedLabel");

		_loader = Globals.LevelLoader;
	}

	public void SetSeed(string seed)
	{
		_seedLabel.Text = seed;
	}
	public void SetTimeScore(float timeSeconds)
	{
		_youTimeLabel.Text = TimeSpan.FromSeconds(timeSeconds).ToString(@"ss\.FF");
		_parTimeLabel.Text = "";
	}

	public void SetConnectionScore(int connections)
	{
		_youHopsLabel.Text = connections.ToString();
		_parHopsLabel.Text = "";
	}

	public void Show()
	{
		GetChild<TextureRect>(0).Show();
	}

	public void Hide()
	{
		GetChild<TextureRect>(0).Hide();
	}

	public void OnRetryButtonPressed()
	{
		Globals.LevelLoader.ReloadLevel();
	}

	public void OnMainMenuButtonPressed()
	{
		Globals.LevelLoader.LoadMenu("res://MainMenu.tscn");
	}

	public void OnNextLevelButtonPressed()
	{
		Globals.LevelLoader.LoadLevel("res://worlds/World.tscn");
	}
}
