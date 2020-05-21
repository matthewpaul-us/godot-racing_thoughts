using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PersonStateMachine : AbstractStateMachine<Person>
{
	protected float _timeLeftToWait = 0;
	private AugmentedRandom _rand;
	private Vector2 _direction = Vector2.Zero;

	public override void _Ready()
	{
		base._Ready();

		_rand = Globals.LevelLoader.Random;

		AddState("wait");
		AddState("walk");
		AddState("picked");

		CallDeferred("SetState", "wait");
	}
	protected override void EnterState(string newState, string oldState)
	{
		switch (newState)
		{
			case "walk":
				_timeLeftToWait = (float)_rand.NextDouble() * 2 + 2;
				_direction = Vector2.One.Rotated(2 * (float)Math.PI * (float)_rand.NextDouble());
				_parent.Velocity = _direction;
				_parent.PlayAnimation("walk");
				break;
			case "wait":
				_timeLeftToWait = (float)_rand.NextDouble() * 5 + 2;
				_parent.PlayAnimation(null);
				_parent.Velocity = Vector2.Zero;
				break;
			case "picked":
				_parent.PlayAnimation(null);
				break;
			default:
				break;
		}
	}

	protected override void ExitState(string oldState, string newState)
	{ }

	protected override void ProcessState(float delta)
	{
		switch (_state)
		{
			case "wait":
				_timeLeftToWait -= delta;
				break;
			case "walk":
				_timeLeftToWait -= delta;
				break;
			default:
				break;
		}
	}

	protected override string ProcessTransition(float delta)
	{
		switch (_state)
		{
			case "wait":
				if (_timeLeftToWait < 0)
				{

					return "walk";
				}
					break;
			case "walk":
				if(_timeLeftToWait < 0)
				{
					return "wait";
				}
				break;
			case "picked":
				if (!_parent.HasFocus)
				{
					return _previousState;
				}
				break;

			default:
				break;
		}

		return _state;
	}
}
