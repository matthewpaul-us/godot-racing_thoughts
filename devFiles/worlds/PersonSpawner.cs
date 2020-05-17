using Godot;
using RacingThoughts.misc;
using System;

public class PersonSpawner : Node
{
	[Export] public PackedScene PersonInstance;
	[Export] public TextureRect SpawnArea;
	[Export] public int NumberToSpawn;

	private Random _rand;
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

			Owner.AddChild(newPerson);
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
