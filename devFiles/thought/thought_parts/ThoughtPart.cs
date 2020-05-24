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
	public List<string> parts = new List<string>();

	private AugmentedRandom _rand = RandomSingleton.GetInstance();
	private TextureButton _button;
	private Node _fontSymbol;
	private AnimationPlayer _anim;

	public override void _Ready()
	{

		_button = GetNode<TextureButton>("TextureButton");
		_button.Connect("button_down", this, nameof(OnThoughtPartClicked));
		_fontSymbol = GetNode("FontAwesome");
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");

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
		EmitSignal(nameof(Clicked), this);
	}

	public void SetFontSymbol(string symbolName)
	{
		_fontSymbol.Set("icon_name", symbolName);
		Part = symbolName;
	}

	public void Shake()
	{
		_anim.Play("shake");
	}

	public void Roll()
	{
		_anim.Play("roll");
	}

}
