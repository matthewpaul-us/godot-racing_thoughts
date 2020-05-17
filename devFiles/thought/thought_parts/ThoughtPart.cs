using Godot;
using Godot.Collections;
using RacingThoughts.misc;
using System;
using System.Linq;

public class ThoughtPart : Node2D
{
	[Signal] public delegate void Clicked(ThoughtPart part);

	[Export]
	public string Part { get; set; }
	[Export]
	private Dictionary<string, string> ThoughtPartDictionary;

	private Random _rand = RandomSingleton.GetInstance();
	private TextureButton _button;

	public override void _Ready()
	{
		Randomize();

		_button = GetNode<TextureButton>("TextureButton");
		_button.Connect("button_down", this, nameof(OnThoughtPartClicked));
	}

	public void Randomize()
	{
		var keys = ThoughtPartDictionary.Keys.ToList();
		var randomThoughtKey = keys[_rand.Next(keys.Count)];

		Part = randomThoughtKey;

		var button = GetNode<TextureButton>("TextureButton");
		button.TextureNormal = GD.Load<Texture>(ThoughtPartDictionary[Part]);
	}

	public void OnThoughtPartClicked()
	{
		GD.Print($"{Part} clicked!");
		EmitSignal(nameof(Clicked), this);
	}

}
