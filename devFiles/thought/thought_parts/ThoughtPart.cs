using Godot;
using Godot.Collections;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class ThoughtPart : Node2D
{
	[Signal] public delegate void Clicked(ThoughtPart part);

	[Export]
	public string Part { get; set; }
	[Export]
	private Godot.Collections.Dictionary<string, string> ThoughtPartDictionary;

	[Export]
	private List<string> parts = new List<string>();

	private AugmentedRandom _rand = RandomSingleton.GetInstance();
	private TextureButton _button;
	private Node _fontSymbol;

	public override void _Ready()
	{

		_button = GetNode<TextureButton>("TextureButton");
		_button.Connect("button_down", this, nameof(OnThoughtPartClicked));
		_fontSymbol = GetNode("FontAwesome");

		Randomize();
	}

	public void Randomize()
	{
		var symbol = _rand.Random(parts);
		Part = symbol;

		SetFontSymbol(symbol);
	}

	public void OnThoughtPartClicked()
	{
		GD.Print($"{Part} clicked!");
		EmitSignal(nameof(Clicked), this);
	}

	private void SetFontSymbol(string symbolName)
	{
		_fontSymbol.Set("icon_name", symbolName);
	}

}
