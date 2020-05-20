using Godot;
using System;

public class TargetCamera : Node2D
{
	public Person FocusPerson;
	public Person TargetPerson;

	private RayCast2D _ray;
	private AnimationPlayer _anim;

	public override void _Ready()
	{
		_ray = GetNode<RayCast2D>("RayCast2D");
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		_anim.Play("default");
	}

	public override void _PhysicsProcess(float delta)
	{
		// Make sure we have both a focus and a target
		if(FocusPerson == null || TargetPerson == null)
		{
			return;
		}

		var direction = TargetPerson.GlobalPosition - FocusPerson.GlobalPosition;

		Position = FocusPerson.GlobalPosition;
		LookAt(TargetPerson.GlobalPosition);

		_ray.Scale = Vector2.One * direction.Length();
		_ray.ForceRaycastUpdate();

		if(_ray.IsColliding())
		{
			var point = _ray.GetCollisionPoint();
			Position = point;
		}
		else
		{
			var finalPosition = TargetPerson.GlobalPosition;
			finalPosition.y += 75;
			Position = finalPosition;
			RotationDegrees = -90.0f;
		}

	}

}
