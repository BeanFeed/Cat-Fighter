using CatFighter.Resources.Scripts;
using Godot;
using System;

public class MobileSup : CheckButton
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private bool toggled = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var globals = GetNode<Globals>("/root/Globals");
        if (globals.mobileOn)
        {
            Pressed = true;
        }
        else
        {
            Pressed = false;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.


    private void _toggled(bool tog)
    {
        var globals = GetNode<Globals>("/root/Globals");
        globals.mobileOn = tog;
    }
}