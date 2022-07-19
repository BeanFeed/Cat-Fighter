using Godot;
using System;

public class Level : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private int maxHearts = 2;
    private int currentHearts = 0;
    private Random rand = new Random();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Node2D>("HeartSpawnLocations").Visible = false;
    }
    private Vector2 GetSpawnPos()
    {
        var SpawnList = GetNode<Node2D>("HeartSpawnLocations").GetChildren();
        var spawnPos = SpawnList[rand.Next(0, SpawnList.Count)];
        return ((Sprite)spawnPos).Position;

    }
    public void RemoveHeartCount() { currentHearts--; }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var HeartTree = GetNode<Node2D>("Hearts");
        if (currentHearts < maxHearts && rand.Next(1, 500) == 25)
        {
            var heart = GD.Load<PackedScene>("res://Resources/Objects/Game/Heart.tscn").Instance<Node2D>();
            heart.Position = GetSpawnPos();
            HeartTree.AddChild(heart);
            currentHearts++;
        }
    }
}
