using Godot;
using System;

public class LevelStartCutscene : CanvasLayer
{
	private Camera2D _camera;
	private Sprite _josephSprite;
	private Sprite _otherSprite;
	private AnimationPlayer _anim;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		_josephSprite = GetNode<Sprite>("Joseph");
		_otherSprite = GetNode<Sprite>("Other");
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void SetupCamera(Vector2 position)
	{
		GD.Print(_camera.Position);
		_camera.Position = position;
		GD.Print(_camera.Position);
		_camera.Current = true;
	}

	internal void UsePeople(Person focusPerson, Person targetPerson)
	{
		_otherSprite.Texture = targetPerson.GetTexture();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//
	//  }
}
