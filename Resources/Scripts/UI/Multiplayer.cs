using CatFighter.Resources.Scripts;
using Godot;
using System;

public class Multiplayer : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var globals = GetNode<Globals>("/root/Globals");
        if (globals.mobileOn)
        {
            Disabled = true;
        }
        else
        {
            Disabled = false;
        }  
    }
    private void _pressed()
    {
        GetTree().ChangeScene("res://Resources/Scenes/GameView.tscn");
    }
}
