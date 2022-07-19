using CatFighter.Resources.Scripts;
using Godot;
using System;

public class MobileCon : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Vector2 prevPos = Vector2.Zero;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (GetNode<Globals>("/root/Globals").mobileOn)
        {
            Visible = true;
        }
        else
        {
            Visible = false;
        }
    }
    private void HandleMovement(Vector2 val)
    {
        val.y *= -1;
        int quad = 0;
        if (val.x > 0 && val.y > 0)
        {
            quad = 1;
        }else if (val.x < 0 && val.y > 0)
        {
            quad = 2;
        }else if (val.x < 0 && val.y < 0)
        {
            quad = 3;
        }else if (val.x > 0 && val.y < 0)
        {
            quad = 4;
        }
        float deg = Mathf.Atan(Mathf.Abs(val.y) / Mathf.Abs(val.x));
        GD.Print(deg);
        if (deg <= 0.75f)
        {
            if (quad == 1 || quad == 4)
            {
                Input.ActionPress("ui_right");
                Input.ActionPress("ui_rightMain");
            }
            else if (quad == 2 || quad == 3)
            {
                Input.ActionPress("ui_left");
                Input.ActionPress("ui_leftMain");
            }
        }else if (deg > 0.75 && (quad == 1 || quad == 2))
        {
            Input.ActionPress("ui_up");
            Input.ActionPress("ui_upMain");
        }
        else
        {
            Input.ActionRelease("ui_right");
            Input.ActionRelease("ui_rightMain");
            Input.ActionRelease("ui_left");
            Input.ActionRelease("ui_leftMain");
            Input.ActionRelease("ui_up");
            Input.ActionRelease("ui_upMain");
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var stick = GetNode<Node>("Joystick/button");
        var val = stick.Call("get_val");
        if (val != null)
        {
            if(prevPos != (Vector2)val)
            HandleMovement((Vector2)val);
            prevPos = (Vector2)val;
        }
    }
    private void _on_Punch_pressed()
    {
        Input.ActionPress("punch");
        Input.ActionPress("punchMain");
    }
    private void _on_Kick_pressed()
    {
        Input.ActionPress("kick");
        Input.ActionPress("kickMain");
    }
    private void _on_Fire_pressed()
    {
        Input.ActionPress("fire");
        Input.ActionPress("fireMain");
    }
}
