using Godot;
using System;

public class NetPlayer : Player
{

    public enum MovDir {
        Left,
        Right,
        None
    }
    public enum Actions {
        Jump,
        Punch,
        Kick,
        Fire,
        None
    }
    private protected MovDir MovingDirection = MovDir.None;
    private protected Actions CurrentAction = Actions.None;
    public override void _Ready()
    {
        speed *= 5;
        drag *= 5;
        jumpPower *= -100;
        player = GetNode<AnimatedSprite>("Sprite");

        isFacingLeft = player.FlipH;
        PunchSound = GetNode<AudioStreamPlayer>("Punch");
        KickSound = GetNode<AudioStreamPlayer>("Kick");
    }
    public void ChangeDirection(MovDir direction) {
        MovingDirection = direction;
    }
    public void ChangeAction(Actions action) {
        CurrentAction = action;
    }
    public MovDir GetDirection() {
        return MovingDirection;
    }
    public Actions GetAction() {
        return CurrentAction;
    }
    
    protected override void HandleMovement(float delta)
    {
        if (GetDirection() == MovDir.Left)
        {
            if (velocity.x > 0)
            {
                velocity.x -= (float)Accel;
            }
            if (velocity.x > -speed)
            {
                velocity.x -= (float)Accel;
            }
            if (player.Animation != "Walk" && !ActionAnimationRunning())
            {
                player.Play("Walk");
            }
        }
        else if (GetDirection() == MovDir.Right)
        {
            if (velocity.x < 0)
            {
                velocity.x += (float)Accel;
            }
            if (velocity.x < speed)
            {
                velocity.x += (float)Accel;
            }
            if (player.Animation != "Walk" && !ActionAnimationRunning())
            {
                player.Play("Walk");
            }

        }
        else
        {
            if (Math.Abs(velocity.x) < (drag))
            {
                if (velocity.x < 0)
                {
                    velocity.x += -velocity.x;
                }
                else if (velocity.x > 0)
                {
                    velocity.x -= velocity.x;
                }
            }
            else
            {
                if (velocity.x < 0)
                {
                    velocity.x += drag;
                }
                else if (velocity.x > 0)
                {
                    velocity.x -= drag;
                }
            }
        }
        if (GetAction() == Actions.Jump && IsOnFloor())
        {
            ChangeAction(Actions.None);
            velocity.y = jumpPower;
            if (player.Animation != "Jump" && !ActionAnimationRunning())
            {
                player.Play("Jump");
            }
        }
        else
        {
            if (velocity.y < 3000)
            {
                velocity.y += gravity;
            }
            else { velocity.y = 3000; }
        }
        if (justHit)
        {
            justHit = false;
            velocity.x = knockback;
        }
        velocity = MoveAndSlide(velocity, Vector2.Up);
        if (velocity != Vector2.Zero)
        {
            if (player.Animation != "Walk" && !ActionAnimationRunning())
            {
                player.Play("Walk");
            }
        }
        if (velocity.x == 0 && player.Animation != "Idle" && !ActionAnimationRunning())
        {
            player.Play("Idle");
        }
        if (player.Animation == "Jump" && IsOnFloor() && !(player.Animation == "Kick" || player.Animation == "Punch" || player.Animation == "Fire"))
        {
            player.Play("Idle");
        }
    }
    
    protected override void HandleActions()
    {
        var punchbox = GetNode<Area2D>("punchbox");
        var collider = GetNode<CollisionShape2D>("punchbox/CollisionShape2D");
        if (GetAction() == Actions.Punch && !IsPunching())
        {
            ChangeAction(Actions.None);
            Punch();
        }
        if (IsPunching() && player.Frame == 5)
        {
            ResetPunchbox();
        }else if (IsPunching() && player.Frame == 3)
        {
            collider.Disabled = false;
            if (!isFacingLeft)
            {
                punchbox.Position = new Vector2(8, 0);
            }
            else
            {
                punchbox.Position = new Vector2(-8, 0);
            }
            punchbox.Scale = new Vector2(5, 1);
        }
        if (GetAction() == Actions.Kick && !IsKicking())
        {
            ChangeAction(Actions.None);
            Kick();
        }
        if (GetAction() == Actions.Fire && !IsFiring() && !justFired)
        {

            Fire();
        }
        if (IsKicking() && player.Frame == 6)
        {
            ResetPunchbox();
        }
        else if (IsKicking() && player.Frame == 4)
        {
            collider.Disabled = false;
            if (!isFacingLeft)
            {
                punchbox.Position = new Vector2(6, 0);
            }
            else
            {
                punchbox.Position = new Vector2(-6, 0);
            }
            punchbox.Scale = new Vector2(5, 1);
        }
        if (IsFiring() && player.Frame == 11 && !justFired)
        {
            var ball = GD.Load<PackedScene>("res://Resources/Objects/Game/Fireball.tscn").Instance<Fireball>();
            ball.owner = Name;
            if (!isFacingLeft)
            {
                ball.Position = new Vector2(Position.x + 10, Position.y);
            }
            else
            {
                ball.Position = new Vector2(Position.x - 10, Position.y);
                ball.goingLeft = true;
            }
            GetParent().AddChild(ball);
            justFired = true;
        }
    }
}
