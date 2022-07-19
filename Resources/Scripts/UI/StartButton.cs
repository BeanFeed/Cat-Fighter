using Godot;
using System;

public class StartButton : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Panel>("/root/Menu/UI/StartPanel").Visible = false;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            var mousePos = GetViewport().GetMousePosition();
            if (!((219 < mousePos.y) && (mousePos.y < 258)))
            {
                GetNode<Panel>("/root/Menu/UI/StartPanel").Visible = false;
            }
            
        }
    private void _pressed()
    {
        GetNode<Panel>("/root/Menu/UI/StartPanel").Visible = true;
    }
}
