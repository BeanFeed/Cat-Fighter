using Godot;
using System;
using System.Linq;

public class MovingPlatform : KinematicBody2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("Player").Play("RisingPlatform");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
