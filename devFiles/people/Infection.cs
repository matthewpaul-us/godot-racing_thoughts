using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Infection : Node
{
	[Signal] public delegate void Infected(Person person);

	[Export] public float MinInfectionTickSeconds;
	[Export] public float MaxInfectionTickSeconds;


	public bool IsRunning = false;
	public List<Person> HealthyPeople = new List<Person>();
	public List<Person> InfectedPeople = new List<Person>();

	[Export] public List<string> InfectedThoughts = new List<string>();

	public Person PatientZero { get { return InfectedPeople[0]; } }
	public float InfectionTimerPercent;

	private float _timeSinceLastInfection = 0;

	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(float delta)
	{
		if(!IsRunning)
		{
			return;
		}

		_timeSinceLastInfection += delta;

		var timeTillInfectionTickNeeded = Mathf.Lerp(MaxInfectionTickSeconds,
			MinInfectionTickSeconds, InfectionTimerPercent);

		timeTillInfectionTickNeeded = Mathf.Clamp(timeTillInfectionTickNeeded,
			MinInfectionTickSeconds,
			MaxInfectionTickSeconds);

		if(_timeSinceLastInfection > timeTillInfectionTickNeeded)
		{
			InfectTick();
			_timeSinceLastInfection = 0;
		}
	}

	public void InfectTick()
	{
		List<Person> peopleToInfect = new List<Person>();

		// Can I jump to an infected person?
		var potentialInfections = HealthyPeople
			.OrderBy(p => PatientZero.GlobalPosition.DistanceSquaredTo(p.GlobalPosition))
			;

		foreach (var person in potentialInfections)
		{
			peopleToInfect.Add(person);

			if (peopleToInfect.Any())
			{
				break;
			}
		}

		if (!peopleToInfect.Any())
		{
			SetPhysicsProcess(false);
			return;
		}

		foreach (var person in peopleToInfect)
		{
			Infect(person);
		}
	}

	public void Infect(Person personToInfect)
	{
		InfectedPeople.Add(personToInfect);
		HealthyPeople.Remove(personToInfect);
		personToInfect.Infect(InfectedThoughts);

		EmitSignal(nameof(Infected), personToInfect);
	}

	public void StartInfection(Person patientZero)
	{
		IsRunning = true;
		Infect(patientZero);
	}
}
