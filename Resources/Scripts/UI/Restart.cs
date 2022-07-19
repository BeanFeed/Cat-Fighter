using Godot;
using System;

public class Restart : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    private void _pressed()
    {
        if (GetParent().GetParent().GetParent().GetParent().Name == "Render")
        {
            GetTree().ChangeScene("res://Resources/Scenes/GameView.tscn");
        }
        else
        {
            GetTree().ChangeScene("res://Resources/Scenes/GameViewSingle.tscn");
        }
    }
}
