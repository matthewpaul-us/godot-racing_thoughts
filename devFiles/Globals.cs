using Godot;
using System;

public class Globals : Node
{
    public static bool HasPlayedTutorial = false;
    public static LevelLoader LevelLoader { get; private set; }
    public override void _Ready()
    {
        var loader = GetNode("/root/LevelLoader") as LevelLoader;

        if(loader == null)
        {
            GD.PrintErr("Globals couldn't find Level Loader");
        }
        else
        {
            LevelLoader = loader;
        }
    }
}
