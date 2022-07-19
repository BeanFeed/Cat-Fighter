using CatFighter.Resources.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public class BotPlayer : Player
{
    [Export]
    private int speed = 50;
    [Export]
    private int drag = 3;
    [Export]
    private int jumpPower = 10;
    [Export]
    private double Accel = 0.5;
    [Export]
    private bool AiEnabled = true;
    private string currentInput = "";
    private float aiTick = 0.05f;
    private bool debugMode = false;
    private bool isCloseLeft;
    private bool isCloseRight;
    private bool canFallLeft;
    private bool canFallRight;
    private bool isPlatformAbove = false;
    private bool isPlatformAboveLeft = false;
    private bool isPlatformAboveRight = false;
    private bool isPlatformLeft;
    private bool isPlatformRight;
    private bool goToEdge = false;
    private bool goUpPlat = false;
    private bool goToLeftEdge = false;
    private bool goUpLeftPlat = false;
    private string movDir = "";
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
        ctrl = altCtrl;
        speed *= 5;
        drag *= 5;
        jumpPower *= -100;
        player = GetNode<AnimatedSprite>("Sprite");
        PunchSound = GetNode<AudioStreamPlayer>("Punch");
        KickSound = GetNode<AudioStreamPlayer>("Kick");
    }

    /*
    private bool CanJumpDown(RayCast2D ray)
    {
        
    }
    */
    private void MovLeft()
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
    private void MovRight()
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
    private void MovJump()
    {
        velocity.y = jumpPower;
        if (player.Animation != "Jump" && !ActionAnimationRunning())
        {
            player.Play("Jump");
        }
    }
    private void HandleMovement(float delta)
    {
        if (movDir == "left")
        {
            MovLeft();
        }
        else if (movDir == "right")
        {
            MovRight();
        }
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
        if (velocity.y < 3000)
        {
            velocity.y += gravity;
        }
        else { velocity.y = 3000; }
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
    private void Fire()
    {
        if (!ActionAnimationRunning() && !IsFiring() && !justFired)
        {
            player.Play("Fire");
        }
    }
    private protected void Kick()
    {
        if (!ActionAnimationRunning() && !IsKicking())
        {
            player.Play("Kick");
            KickSound.Play();

        }
    }
    private protected void HandleActions()
    {
        var punchbox = GetNode<Area2D>("punchbox");
        var collider = GetNode<CollisionShape2D>("punchbox/CollisionShape2D");
        if (IsPunching() && player.Frame == 5)
        {
            ResetPunchbox();
        }
        else if (IsPunching() && player.Frame == 3)
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
            var targ = GetParent().GetNode<Player>("RedPlayer");
            bool shootLeft = false;
            if (targ.Position.x < Position.x)
            {
                shootLeft = true;
            }

            ball.owner = Name;
            if (!shootLeft)
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
    private void HandleDetectionLoc()
    {
        var decNode = GetNode<Node2D>("Detection");
        if (isFacingLeft)
        {
            decNode.Position = new Vector2(0, 0);
        }
        else
        {
            decNode.Position = new Vector2(-4,0);
        }
    }
    bool curTEdgeL = false;
    private void TryShoot()
    {
        if (IsCharged())
        {
            Fire();
        }
    }
    private void GoDownEdge()
    {
        if (!goToEdge)
        {
            goToEdge = true;
            if (goToLeftEdge)
            {
                curTEdgeL = true;
                
            }
        }

        if (curTEdgeL)
        {

            if (!isCloseLeft)
            {
                movDir = "left";
            }
            else if (canFallLeft)
            {
                movDir = "left";

            }
            else
            {
                curTEdgeL = !curTEdgeL;
            }
        }
        else
        {
            if (!isCloseRight)
            {
                movDir = "right";
            }else if (canFallRight)
            {
                movDir = "right";

            }
            else
            {
                curTEdgeL = !curTEdgeL;
            }
        }
        if (!IsOnFloor())
        {
            goToEdge = false;
            curTEdgeL = false;
            movDir = "";
        }
    }
    bool curTPlatL = false;
    private void GoUpPlat()
    {
        if (!goUpPlat)
        {
            goUpPlat = true;
            if (goUpLeftPlat)
            {
                curTPlatL = true;

            }
        }

        if (curTPlatL)
        {
            if (!isCloseLeft)
            {
                movDir = "left";
                if ((isPlatformAboveLeft || isPlatformAbove) && IsOnFloor())
                {
                    MovJump();
                }
            }else if (isCloseLeft && !(isPlatformAbove || isPlatformAboveLeft || isPlatformAboveRight))
            {
                curTPlatL = !curTPlatL;
            }
        }
        else
        {
            if (!isCloseRight)
            {
                movDir = "right";
                if ((isPlatformAboveRight || isPlatformAbove) && IsOnFloor())
                {
                    MovJump();
                }
            }
            else if (isCloseRight && !(isPlatformAbove || isPlatformAboveLeft || isPlatformAboveRight))
            {
                curTPlatL = !curTPlatL;
            }
        }

        if (!IsOnFloor() && velocity.y > 1)
        {
            goUpPlat = false;
            curTPlatL = false;
            movDir = "";
        }
    }
    private Node2D GetTarget()
    {
        if (GetHealth() < GetParent().GetNode<Player>("RedPlayer").GetHealth()-20 && GetParent().GetNode<Node2D>("Hearts").GetChildren().Count > 0)
        {
            return (Node2D)GetParent().GetNode<Node2D>("Hearts").GetChildren()[0];
        }
        else { return GetParent().GetNode<Player>("RedPlayer"); }
    }
    private Random rand = new Random();
    private void TravelToTarget(Node2D target)
    {
        var tPos = target.Position;
        var pos = Position;
        bool isToLeft = false;
        bool isAbove = false;
        if (tPos.x < pos.x)
        {
            isToLeft = true;
        }
        if (tPos.y < pos.y)
        {
            isAbove = true;
        }
        bool shouldGoUp = false;
        bool shouldGoDown = false;
        if (!Utils.Within(tPos.y, pos.y))
        {
            if (isAbove)
            {
                shouldGoUp = true;
            }
            else
            {
                shouldGoDown = true;
            }
        }
        bool shouldGoLeft = false;
        bool shouldGoRight = false;
        int closeDist = 60;
        if (target is Heart)
        {
            closeDist = 1;
        }
        if (Math.Abs(pos.x - tPos.x) > closeDist)
        {
            if (isToLeft)
            {
                shouldGoLeft = true;
            }
            else
            {
                shouldGoRight = true;
            }
        }
        else
        {
            var toPunch = rand.Next(1, 6);
            if (toPunch == 1)
            {
                Punch();
            }
            else if(toPunch == 3)
            {
                Kick();
            }
        }

        if (shouldGoUp && IsOnFloor())
        {
            if (shouldGoLeft)
            {
                goUpLeftPlat = true;
            }
            else
            {
                goUpLeftPlat = false;
            }
            GoUpPlat();
            return;
        }

        if (shouldGoDown)
        {
            if (shouldGoLeft)
            {
                goToLeftEdge = true;
            }
            else
            {
                goToLeftEdge = false;
            }
            GoDownEdge();
            return;
        }

        if (shouldGoLeft)
        {
            movDir = "left";
            return;
        }else if (shouldGoRight)
        {
            movDir = "right";
            return;
        }
        else
        {
            movDir = "";
        }
        

    }
    private void GetDistTarget(Node2D target)
    {
        var tPos = target.Position;
        var pos = Position;
        bool isToLeft = false;
        bool isAbove = false;
        if (tPos.x < pos.x)
        {
            isToLeft = true;
        }
        if (tPos.y < pos.y)
        {
            isAbove = true;
        }
        bool shouldGoUp = true;
        bool shouldGoDown = true;
        if (!Utils.Within(tPos.y, pos.y))
        {
            if (isAbove || pos.y > tPos.y - 20)
            {
                shouldGoUp = false;
            }
            else
            {
                shouldGoDown = false;
            }
        }

        bool shouldGoLeft = true;
        bool shouldGoRight = true;
        int closeDist = 162;
        if (target is Heart)
        {
            closeDist = 1;
        }
        
        if (isToLeft)
        {
            shouldGoLeft = false;
        }
        else
        {
            shouldGoRight = false;
        }
        if (shouldGoUp && (isPlatformAbove || isPlatformAboveLeft || isPlatformAboveRight) && IsOnFloor())
        {
            MovJump();
            if (isPlatformAboveRight)
            {
                movDir = "right";
            }
            else if (isPlatformAboveLeft)
            {
                movDir = "left";
            }
            return;
        }

        if (shouldGoDown)
        {
            if (shouldGoLeft)
            {
                goToLeftEdge = true;
            }
            else
            {
                goToLeftEdge = false;
            }
            GoDownEdge();
            return;
        }

        if (shouldGoLeft && !isCloseLeft)
        {
            movDir = "left";
            return;
        }
        else if (shouldGoRight && !isCloseRight)
        {
            movDir = "right";
            return;
        }
        else
        {
            movDir = "";
            return;
        }
    }
    private void TravelToFirePos(Node2D target)
    {
        var tPos = target.Position;
        var pos = Position;
        bool isToLeft = false;
        bool isAbove = false;
        if (tPos.x < pos.x)
        {
            isToLeft = true;
        }
        if (tPos.y < pos.y)
        {
            isAbove = true;
        }
        bool shouldGoUp = false;
        bool shouldGoDown = false;
        if (!Utils.Within(tPos.y, pos.y))
        {
            if (isAbove)
            {
                shouldGoUp = true;
            }
            else
            {
                shouldGoDown = true;
            }
        }
        bool shouldGoLeft = false;
        bool shouldGoRight = false;
        int closeDist = 300;

        if (Math.Abs(pos.x - tPos.x) > closeDist)
        {
            if (isToLeft)
            {
                shouldGoLeft = true;
            }
            else
            {
                shouldGoRight = true;
            }
        }

        if (shouldGoUp && IsOnFloor())
        {
            if (shouldGoLeft)
            {
                goUpLeftPlat = true;
            }
            else
            {
                goUpLeftPlat = false;
            }
            GoUpPlat();
            return;
        }

        if (shouldGoDown)
        {
            if (shouldGoLeft)
            {
                goToLeftEdge = true;
            }
            else
            {
                goToLeftEdge = false;
            }
            GoDownEdge();
            return;
        }

        if (shouldGoLeft)
        {
            movDir = "left";
            return;
        }
        else if (shouldGoRight)
        {
            movDir = "right";
            return;
        }
        else
        {
            movDir = "";
        }
        if (Utils.Within(tPos.y, pos.y))
        {
            TryShoot();
        }
    }
    private void HandleAi()
    {
        var LeftDown = GetNode<RayCast2D>("Detection/LeftDown");
        var RightDown = GetNode<RayCast2D>("Detection/RightDown");
        var Up = GetNode<RayCast2D>("Detection/Up");
        var UpLeft = GetNode<RayCast2D>("Detection/UpLeft");
        var UpRight = GetNode<RayCast2D>("Detection/UpRight");
        var PLeft = GetNode<RayCast2D>("Detection/FindPlatToLeft");
        var PRight = GetNode<RayCast2D>("Detection/FindPlatToRight");
        var target = GetTarget();
        LeftDown.CastTo = new Vector2(0, 15);
        RightDown.CastTo = new Vector2(0, 15);
        RightDown.ForceRaycastUpdate();
        LeftDown.ForceRaycastUpdate();
        isCloseLeft = !LeftDown.IsColliding();
        isCloseRight = !RightDown.IsColliding();
        canFallLeft = false;
        canFallRight = false;
        isPlatformLeft = PLeft.IsColliding();
        isPlatformRight = PRight.IsColliding();
        
        LeftDown.CastTo = new Vector2(0, 210);
        RightDown.CastTo = new Vector2(0, 210);
        RightDown.ForceRaycastUpdate();
        LeftDown.ForceRaycastUpdate();
        if (isCloseLeft && LeftDown.IsColliding())
        {
            canFallLeft = true;   
        }
        if (isCloseRight && RightDown.IsColliding())
        {
            canFallRight = true;
        }
        isPlatformAbove = Up.IsColliding();
        isPlatformAboveLeft = UpLeft.IsColliding();
        isPlatformAboveRight = UpRight.IsColliding();
        bool shouldKeepDistance = true;
        if ( !IsCharged() || Utils.Dist(target.Position, this.Position) > 224)
        {
            shouldKeepDistance = false;
        }
        if (GetHealth() < GetParent().GetNode<Player>("RedPlayer").GetHealth() - 20 && target is Player)
        {
            GetDistTarget(target);
        }else if (target is Player && !IsCharged())
        {
            TravelToFirePos(target);
        }
        else
        {
            TravelToTarget(target);
        }
        
        
    }
    
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        var Render = GetNode<GameView>("/root/RenderSingle");
        HandleDeath();
        

        if (!isDead && !Render.GameEnded())
        {
            aiTick -= delta;
            if (AiEnabled && aiTick <= 0)
            {
                HandleAi();
                aiTick = 0.05f;
            }
            if (flashTimer > 0)
            {
                flashTimer -= delta;
            }
            if (flashTimer <= 0)
            {
                ((ShaderMaterial)player.Material).SetShaderParam("flash_modifier", 0.0f);
            }
            HandleDetectionLoc();
            HandleHitBox();
            if (debugMode)
            {
                base.HandleMovement(delta);
            }
            else
            {
                HandleMovement(delta);
            }
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

}
