using Godot;
using RacingThoughts.misc;
using System;
using System.Threading.Tasks;

public class LevelLoader : Node2D
{
	[Signal] public delegate void SceneChangeFinished();
	public Node LoadedScene { get; set; }
	public string LoadedScenePath { get; set; }
	public AugmentedRandom Random { get; set; }

	private Tween _tween;
	private ColorRect _curtain;
	private Label _levelLabel;

	private Color _curtainOriginalColor;

	public override void _Ready()
	{
		_tween = GetNode<Tween>("Tween");
		_curtain = GetNode<ColorRect>("CanvasLayer/Curtain");
		_levelLabel = GetNode<Label>("CanvasLayer/LevelLabel");

		Random = new AugmentedRandom();
		RandomSingleton.SetInstance(Random);

		LoadMenu("res://MainMenu.tscn");
	}

	public void ReloadLevel()
	{
		LoadLevel(LoadedScenePath, Random.Seed);
	}

	public async void LoadLevel(string resourcePath, string seed = null)
	{
		await LowerCurtain();
		if(LoadedScene != null)
		{
			RemoveChild(LoadedScene);
			LoadedScene.CallDeferred("QueueFree");
		}

		Random = new AugmentedRandom(seed ?? Random.NextSeed());
		RandomSingleton.SetInstance(Random);

		await ShowMessage(Random.Seed);

		var level = GD.Load<PackedScene>(resourcePath).Instance();

		AddChild(level);

		LoadedScene = level;
		LoadedScenePath = resourcePath;

		await RaiseCurtain();

		EmitSignal(nameof(SceneChangeFinished));
	}

	public void LoadMenu(string resourcePath)
	{
		if(LoadedScene != null)
		{
			RemoveChild(LoadedScene);
			LoadedScene.CallDeferred("QueueFree");
		}

		var level = GD.Load<PackedScene>(resourcePath).Instance();

		AddChild(level);

		LoadedScene = level;
	}

	private async Task LowerCurtain()
	{
		_curtainOriginalColor = _curtain.Color;

		_tween.InterpolateProperty(_curtain, "color", _curtainOriginalColor, Colors.Black, 1.5f,
			Tween.TransitionType.Linear, Tween.EaseType.In);
		_tween.Start();

		await ToSignal(_tween, "tween_completed");
	}

	private async Task RaiseCurtain()
	{
		_tween.InterpolateProperty(_curtain, "color", Colors.Black, _curtainOriginalColor, 1.5f,
			Tween.TransitionType.Linear, Tween.EaseType.In);
		_tween.Start();

		await ToSignal(_tween, "tween_completed");
	}

	private async Task ShowMessage(string message)
	{
		_levelLabel.Text = message;
		_levelLabel.Show();

		await ToSignal(GetTree().CreateTimer(2), "timeout");
		_levelLabel.Hide();
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
	}
}
