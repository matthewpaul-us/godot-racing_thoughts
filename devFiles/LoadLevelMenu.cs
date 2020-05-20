using Godot;
using System;

public class LoadLevelMenu : CanvasLayer
{
	private LevelLoader _loader;
	private TextEdit _levelSeedText;
	public override void _Ready()
	{
		_loader = GetParent<LevelLoader>();
		_levelSeedText = GetNode<TextEdit>("Panel/MarginContainer/VBoxContainer/TextEdit");
	}

	public void OnLoadLevelButtonPressed()
	{
		_loader.LoadLevel("res://worlds/World.tscn", _levelSeedText.Text);
	}
}
