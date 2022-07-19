using Godot;
using System;

public class Download2 : Button
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
        if (Name == "Windows")
        {
            OS.ShellOpen("https://beanfeed.tk/Games/Cat%20Fighter/appDownload/CatFighter(Windows).zip");
        }
        else if (Name == "Linux")
        {
            OS.ShellOpen("https://beanfeed.tk/Games/Cat%20Fighter/appDownload/Cat_Fighter(Linux).zip");
        }
    }
}
