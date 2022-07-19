using Godot;
using System;

public class Cloud : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    private int speed = 10;
    [Export]
    private int startxpos = 1380;
    [Export]
    private int endxpos = 174;
    [Export]
    private bool randSpeed = false;
    private static Random rand = new Random();
    private int randomSpeed = rand.Next(1, 40);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Position = new Vector2(1380, Position.y);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
        if (Position.x < endxpos)
        {
            Position = new Vector2(startxpos, Position.y);
            if (randSpeed)
            {
                randomSpeed = rand.Next(1, 40);
            }
        }
        Vector2 velocity = new Vector2(-speed - randomSpeed, 0);
        MoveAndSlide(velocity);
    }
}
