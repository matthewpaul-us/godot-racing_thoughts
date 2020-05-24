using Godot;
using System;

public class MainMenu : CanvasLayer
{
	private LevelLoader _loader;

	public override void _Ready()
	{
		_loader = GetParent<LevelLoader>();
	}

	public void OnPlayButtonPressed()
	{
		if(Globals.HasPlayedTutorial)
		{
			_loader.LoadLevel("res://worlds/World.tscn");
		}
		else
		{
			_loader.LoadLevel("res://tutorial/Tutorial.tscn", "The Beginning");
		}
	}
	public void OnLevelButtonPressed()
	{
		_loader.LoadMenu("res://LoadLevelMenu.tscn");
	}

	public void OnCreditsButtonPressed()
	{
		_loader.LoadLevel("res://tutorial/Tutorial.tscn", "The Beginning");
	}
}
