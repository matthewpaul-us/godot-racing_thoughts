using Godot;
using System;

public class LevelStartCutscene : CanvasLayer
{
	private Camera2D _camera;
	private Sprite _josephSprite;
	private Sprite _otherSprite;
	private AnimationPlayer _anim;
	private Vector2 _focusOriginalPosition;
	private Vector2 _targetOriginalPosition;
	private Vector2 _focusFinalPosition;
	private Vector2 _targetFinalPosition;
	private Tween _tween;

	private Person _focusPerson;
	private Person _targetPerson;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		_josephSprite = GetNode<Sprite>("Joseph");
		_otherSprite = GetNode<Sprite>("Other");
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		_tween = GetNode<Tween>("Tween");

		Globals.LevelLoader.Connect(nameof(LevelLoader.SceneChangeFinished),
			this, nameof(OnSceneChangeFinished), flags: (uint)ConnectFlags.Oneshot);
	}

	public void OnSceneChangeFinished()
	{
		_anim.Play("scene");
	}

	public void SetupCamera(Vector2 position)
	{
		_camera.Position = position;
		_camera.Current = true;
	}

	internal void UsePeople(Person focusPerson, Person targetPerson)
	{
		_otherSprite.Texture = targetPerson.GetTexture();
		_focusOriginalPosition = _josephSprite.GlobalPosition;
		_targetOriginalPosition = _otherSprite.GlobalPosition;
		_focusFinalPosition = focusPerson.GlobalPosition;
		_targetFinalPosition = targetPerson.GlobalPosition;

		_focusPerson = focusPerson;
		_targetPerson = targetPerson;
	}

	public void StartLerpPeople(float timeToLerp)
	{
		var focusFinalWorldPos = TransformCoordFromScreenToWorld(_focusPerson.GlobalPosition, _camera.Zoom);
		var targetFinalWorldPos = TransformCoordFromScreenToWorld(_targetPerson.GlobalPosition, _camera.Zoom);

		// Location
		_tween.InterpolateProperty(_josephSprite, "global_position", _focusOriginalPosition,
			focusFinalWorldPos, timeToLerp, Tween.TransitionType.Cubic, Tween.EaseType.Out);
		_tween.InterpolateProperty(_otherSprite, "global_position", _targetOriginalPosition,
			targetFinalWorldPos, timeToLerp, Tween.TransitionType.Cubic, Tween.EaseType.Out);

		// Size
		_tween.InterpolateProperty(_josephSprite, "scale", _josephSprite.Scale,
			_josephSprite.Scale / _camera.Zoom, timeToLerp, Tween.TransitionType.Cubic, Tween.EaseType.Out);
		_tween.InterpolateProperty(_otherSprite, "scale", _otherSprite.Scale,
			_otherSprite.Scale / _camera.Zoom, timeToLerp, Tween.TransitionType.Cubic, Tween.EaseType.Out);

		_tween.Start();
	}

	public Vector2 TransformCoordFromScreenToWorld(Vector2 coordinates, Vector2 worldSize)
	{
		return coordinates / worldSize;
	}

	public void SetCameraPositionToJoseph()
	{
		_camera.GlobalPosition = _focusPerson.GlobalPosition;
		//= TransformCoordFromScreenToWorld(_focusPerson.GlobalPosition, _camera.Zoom);
	}
}
