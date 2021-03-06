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
	[Export] public Color PersonSelectedColor;
	[Export] public Color PersonInfectedColor;
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
			SpawnAt(position);
		}
	}

	public Person SpawnAt(Vector2 position, bool isJoseph = false)
	{
		var newPerson = PersonInstance.Instance() as Person;

		newPerson.Position = position;

		Texture texture;
		if (isJoseph)
		{
			texture = PersonTextures.First();
		}
		else
		{
            texture = _rand.Random(PersonTextures);
		}

		SortLayer.AddChild(newPerson);

		var normalColor = Randomize(PersonColor, ColorVariation);
		var shiningColor = Randomize(PersonShineColor, ColorVariation);
		var infectedColor = Randomize(PersonInfectedColor, ColorVariation);

		newPerson.SetColor(normalColor);

		newPerson.NormalColor = normalColor;
		newPerson.ShiningColor = shiningColor;
		newPerson.InfectedColor = infectedColor;
		newPerson.SelectedColor = PersonSelectedColor;

		newPerson.SetTexture(texture);

		return newPerson;
	}

	private Color Randomize(Color color, float variation)
	{
        return color.Lightened((float)_rand.NextDouble() * variation).Blend(color.Darkened((float)_rand.NextDouble() * variation));
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
