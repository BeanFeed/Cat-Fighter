using CatFighter.Resources.Scripts;
using Godot;
using System;

public class GameView : Node2D
{
    private Player redPlayer;
    private Label redHealthLabel;
    private Label redChargeLabel;
    private Player bluePlayer;
    private Label blueHealthLabel;
    private Label blueChargeLabel;
    private bool gameEnded = false;
    public override void _Ready()
    {
        redPlayer = GetNode<Player>("Box/Viewport/Level/RedPlayer");
        redHealthLabel = GetNode<Label>("UI/RedHealth");
        redChargeLabel = GetNode<Label>("UI/RedFireballCharge");
        bluePlayer = GetNode<Player>("Box/Viewport/Level/BluePlayer");
        blueHealthLabel = GetNode<Label>("UI/BlueHealth");
        blueChargeLabel = GetNode<Label>("UI/BlueFireballCharge");
        var popup = GetNode<Panel>("UI/EndScreen");
        popup.Visible = false;
    }

    private void HandleEnding()
    {
        if (redPlayer.IsDead())
        {
            EndGame("Blue", bluePlayer);
        }else if (bluePlayer.IsDead())
        {
            EndGame("Red", redPlayer);
        }
        
    }
    private void EndGame(string name, Player winner)
    {
        gameEnded = true;
        var popup = GetNode<Panel>("UI/EndScreen");
        var winnerLabel = GetNode<Label>("UI/EndScreen/Panel/Winner");
        var punchesLabel = GetNode<Label>("UI/EndScreen/Panel/Punches");
        var kicksLabel = GetNode<Label>("UI/EndScreen/Panel/Kicks");
        var sprite = GetNode<AnimatedSprite>("UI/EndScreen/Panel/Sprite");
        var globals = GetNode<Globals>("/root/Globals");
        winnerLabel.Text = name + " Player Wins";
        punchesLabel.Text = "Punches: " + Convert.ToString(winner.GetPunches());
        kicksLabel.Text = "Kicks: " + Convert.ToString(winner.GetKicks());
        sprite.Animation = name;
        popup.Visible = true;
    }
    private void HandleChargeLabel()
    {
        if (!redPlayer.GetFireballCharged())
        {
            redChargeLabel.Text = "Fireball: Charged";
        }
        else
        {
            redChargeLabel.Text = "Fireball: Recharging";
        }
        if (!bluePlayer.GetFireballCharged())
        {
            blueChargeLabel.Text = "Fireball: Charged";
        }
        else
        {
            blueChargeLabel.Text = "Fireball: Recharging";
        }
    }
    public bool GameEnded() { return gameEnded; }
    public override void _Process(float delta)
    {
        int redHealth = redPlayer.GetHealth();
        redHealthLabel.Text = "Red Player Heatlh: " + Convert.ToString(redHealth);
        int blueHealth = bluePlayer.GetHealth();
        blueHealthLabel.Text = "Blue Player Heatlh: " + Convert.ToString(blueHealth);
        HandleEnding();
        HandleChargeLabel();
    }
}
