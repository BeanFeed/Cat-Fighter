using CatFighter.Resources.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    [Export]
    protected int speed = 50;
    [Export]
    protected int drag = 3;
    [Export]
    protected int jumpPower = 10;
    [Export]
    protected bool useAltControls = false;
    [Export]
    protected double Accel = 0.5;
    protected Dictionary<string, string> mainCtrl = new Dictionary<string, string>();
    protected Dictionary<string, string> altCtrl = new Dictionary<string, string>();
    protected Dictionary<string, string> ctrl;
    protected AnimatedSprite player;
    protected AudioStreamPlayer PunchSound;
    protected AudioStreamPlayer KickSound;
    protected int gravity = Globals.gravity;
    protected int health = 100;
    protected bool isDead = false;
    protected bool isFacingLeft = false;
    protected bool justFired = false;
    protected Vector2 velocity = new Vector2(0,0);
    protected int punches = 0;
    protected int kicks = 0;
    protected float fireCooldown = 5;
    protected float flashTimer = 0;
    protected bool justHit = false;
    protected float knockback = 0;
    public override void _Ready()
    {
        mainCtrl.Add("Left", "ui_leftMain");
        mainCtrl.Add("Right", "ui_rightMain");
        mainCtrl.Add("Jump", "ui_upMain");
        mainCtrl.Add("Punch", "punchMain");
        mainCtrl.Add("Kick", "kickMain");
        mainCtrl.Add("Fire", "fireMain");
        altCtrl.Add("Left", "ui_left");
        altCtrl.Add("Right", "ui_right");
        altCtrl.Add("Jump", "ui_up");
        altCtrl.Add("Punch", "punch");
        altCtrl.Add("Kick", "kick");
        altCtrl.Add("Fire", "fire");
        if (useAltControls)
        {
            ctrl = altCtrl; 
        }else
        {
            ctrl = mainCtrl;
        }
        speed *= 5;
        drag *= 5;
        jumpPower *= -100;
        player = GetNode<AnimatedSprite>("Sprite");

        isFacingLeft = player.FlipH;
        PunchSound = GetNode<AudioStreamPlayer>("Punch");
        KickSound = GetNode<AudioStreamPlayer>("Kick");
    }
    public int GetHealth()
    {
        return health;
    }
    public bool IsPunching()
    {
        if (player.Animation == "Punch" && player.Frames.GetFrameCount(player.Animation) != player.Frame + 1)
        {
            return true;
        }
        return false;
    }
    public bool IsKicking()
    {
        if (player.Animation == "Kick" && player.Frames.GetFrameCount(player.Animation) != player.Frame + 1)
        {
            return true;
        }
        return false;
    }
    public bool IsFiring()
    {
        if (player.Animation == "Fire" && player.Frames.GetFrameCount(player.Animation) != player.Frame + 1)
        {
            return true;
        }
        return false;
    }
    public void Damage(int amount)
    {
        health -= amount;
        GetNode<AudioStreamPlayer>("Hit").Play();
        ((ShaderMaterial)player.Material).SetShaderParam("flash_modifier", 0.472f);
        flashTimer = 0.1f;
    }
    protected bool ActionAnimationRunning()
    {
        if (player.Frames.GetFrameCount(player.Animation) != player.Frame + 1 && (player.Animation == "Jump" || player.Animation == "Kick" || player.Animation == "Punch" || player.Animation == "Fire"))
        {
            return true;
        }
        return false;
    }
    protected void HandleAniDir()
    {
        if (velocity.x < 0)
        {
            isFacingLeft = true;
        }
        else if (velocity.x > 0)
        {
            isFacingLeft = false;
        }
        AnimatedSprite spriteNode = GetNode<AnimatedSprite>("Sprite");
        if (isFacingLeft)
        {
            spriteNode.FlipH = true;
        }
        else
        {
            spriteNode.FlipH = false;
        }
    }
    virtual protected void HandleMovement(float delta)
    {
        if (Input.IsActionPressed(ctrl["Left"]))
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
        else if (Input.IsActionPressed(ctrl["Right"]))
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
        if (Input.IsActionJustPressed(ctrl["Jump"]) && IsOnFloor())
        {
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
    private protected void Punch()
    {
        if (!ActionAnimationRunning())
        {
            player.Play("Punch");
            PunchSound.Play();
        }      
    }
    private protected void Kick()
    {
        if (!ActionAnimationRunning())
        {
            player.Play("Kick");
            KickSound.Play();
            
        }
    }
    private protected void Fire()
    {
        if (!ActionAnimationRunning())
        {
            player.Play("Fire");
        }
    }
    private protected void ResetPunchbox()
    {
        var punchbox = GetNode<Area2D>("punchbox");
        var collider = GetNode<CollisionShape2D>("punchbox/CollisionShape2D");
        punchbox.Position = new Vector2(0, 0);
        punchbox.Scale = new Vector2(1, 1);
        collider.Disabled = true;
    }
    virtual protected void HandleActions()
    {
        var punchbox = GetNode<Area2D>("punchbox");
        var collider = GetNode<CollisionShape2D>("punchbox/CollisionShape2D");
        if (Input.IsActionJustPressed(ctrl["Punch"]) && !IsPunching())
        {
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
        if (Input.IsActionJustPressed(ctrl["Kick"]) && !IsKicking())
        {
            Kick();
        }
        if (Input.IsActionJustPressed(ctrl["Fire"]) && !IsFiring() && !justFired)
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
    private protected bool IsCharged() { return !justFired; }
    private protected void HandleHitBox()
    {
        var hitbox = GetNode<Area2D>("hitbox");
        if (isFacingLeft)
        {
            hitbox.Position = new Vector2(4, 0);
        }
        else
        {
            hitbox.Position = new Vector2(0, 0);
        }
    }
    private protected void HandleDeath()
    {
        if (health <= 0)
        {
            player.Play("Die");
            isDead = true;
        }
        if (health < 0)
        {
            health = 0;
        }
    }
    public bool IsDead() { return isDead; }
    public int GetPunches() { return punches; }
    public int GetKicks() { return kicks; }
    public void AddPunch() { punches++; }
    public void AddKick() { kicks++; }
    public bool GetFireballCharged() { return justFired; }
    public void AddHealth(int Health)
    {
        if ((GetHealth() + Health) < 100)
        {
            health += Health;
        }else
        {
            health = 100;
        }
    }
    private protected void HandleFireCooldown(float delta)
    {
        if (justFired)
        {
            fireCooldown -= delta;
            if (fireCooldown <= 0)
            {
                justFired = false;
                fireCooldown = 5;
            }
        }
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        var Render = (GameView)GetParent().GetParent().GetParent().GetParent();
        HandleDeath();
        if (!isDead && !Render.GameEnded())
        {
            if (flashTimer > 0)
            {
                flashTimer -= delta;
            }
            if (flashTimer <= 0)
            {
                ((ShaderMaterial)player.Material).SetShaderParam("flash_modifier", 0.0f);
            }
            HandleHitBox();
            HandleMovement(delta);
            HandleAniDir();
            HandleActions();
            HandleFireCooldown(delta);
        }
        else
        {
            if (!IsDead())
            {
                player.Animation = "Idle";
            }
        }     
    }
    
    private void _on_hitbox_area_entered(Area2D area)
    {
        if (area.Name == "punchbox" && !isDead)
        {
            if (area.GetParent() is Player pl && pl != this && pl.IsPunching())
            {
                Damage(10);
                justHit = true;
                if (pl.Position.x < Position.x)
                {
                    knockback = 500.0f;
                }
                else
                {
                    knockback = -500.0f;
                }
                pl.AddPunch();
            }
            if (area.GetParent() is Player Pl && Pl != this && Pl.IsKicking())
            {
                Damage(15);
                justHit = true;
                if (Pl.Position.x < Position.x)
                {
                    knockback = 700.0f;
                }
                else
                {
                    knockback = -700.0f;
                }
                Pl.AddKick();
            }

        }else if (area.Name == "FallBox")
        {
            Damage(1000);
        }else if (area.Name == "Heart")
        {
            if (health < 100)
            {
                if ((25 + health) < 100)
                {
                    health += 25;
                }
                else
                {
                    health = 100;
                }
                var level = GetParent();
                ((Level)level).RemoveHeartCount();
                area.GetParent().GetParent().QueueFree();
            }
        }
    }
    
}
