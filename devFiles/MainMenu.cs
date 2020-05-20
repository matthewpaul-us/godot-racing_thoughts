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
		_loader.LoadLevel("res://worlds/World.tscn");
	}
	public void OnLevelButtonPressed()
	{
		_loader.LoadMenu("res://LoadLevelMenu.tscn");
	}
}
