using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class PersonSpawner : Node
{
	[Export] public PackedScene PersonInstance;
	[Export]public List<Texture> PersonTextures;
	[Export] public TextureRect SpawnArea;
	[Export] public int NumberToSpawn;

	[Export] public Color PersonColor;
	[Export] public Color PersonShineColor;
	[Export(PropertyHint.Range, "0,1")] public float ColorVariation;

	public YSort SortLayer;

	private AugmentedRandom _rand;
	public override void _Ready()
	{
		_rand = RandomSingleton.GetInstance();
	}

	public void Spawn()
	{
		for (int i = 0; i < NumberToSpawn; i++)
		{
			var position = GetRandomPosition();
			var newPerson = PersonInstance.Instance() as Person;

			newPerson.Position = position;

			var color = PersonColor;
			color = color.Lightened((float)_rand.NextDouble() * ColorVariation).Blend(color.Darkened((float)_rand.NextDouble() * ColorVariation));

			var shineColor = PersonShineColor;
			shineColor = shineColor.Lightened((float)_rand.NextDouble() * ColorVariation).Blend(shineColor.Darkened((float)_rand.NextDouble() * ColorVariation));

			var texture = _rand.Random(PersonTextures);

			SortLayer.AddChild(newPerson);
			newPerson.SetColor(color);

			newPerson.NormalColor = color;
			newPerson.ShiningColor = shineColor;

			newPerson.SetTexture(texture);
		}
	}

	private Vector2 GetRandomPosition()
	{
		var topLeftPos = SpawnArea.RectPosition;
		var size = SpawnArea.RectSize;

		var randX = (float)_rand.NextDouble() * size.x + topLeftPos.x;
		var randY = (float)_rand.NextDouble() * size.y + topLeftPos.y;

		return new Vector2(randX, randY);
	}
}
