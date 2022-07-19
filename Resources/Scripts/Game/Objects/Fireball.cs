using Godot;
using System;
using System.Collections.Generic;

public class Fireball : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public bool goingLeft = false;
    public string owner;
    private int speed = 500;
    private AnimatedSprite player;
    private List<Player> alreadyHit = new List<Player>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<AnimatedSprite>("Sprite");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.\
    private void HandleAni()
    {
        if (goingLeft)
        {
            player.FlipH = true;
        }
        else
        {
            player.FlipH = false;
        }
        if (player.Frame == 11)
        {
            QueueFree();
        }
        player.Playing = true;
    }
    private void HandleMovement()
    {
        var velocity = new Vector2();
        if (goingLeft)
        {
            velocity.x = -speed;
        }
        else
        {
            velocity.x = speed;
        }
        MoveAndSlide(velocity);
    }
    public override void _Process(float delta)
    {
        HandleAni();
        HandleMovement();
    }
    
    private void _on_Fireball_body_entered(Node body)
    {
        GD.Print(body);
        GD.Print(owner);
        if (body is Player pl && pl.Name != owner && !alreadyHit.Contains(pl))
        {
            alreadyHit.Add(pl);
            pl.Damage(20);
        }
    }
}
