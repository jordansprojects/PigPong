using Godot;
using System;

public class Wolf : Node2D
{
    internal  int direction = 1;
    internal int bounceDirection = 1;
    private float initialHeight;
    private AnimatedSprite anim;
    private DetectionBox db;
    private Timer animTimer;
    private float animationDuration = .20f;
    private Ball anchor;

    RandomNumberGenerator rnd;
    bool idling = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anchor = GetNode<Ball>("../Ball");
        rnd = new RandomNumberGenerator();
        rnd.Randomize();
        initialHeight = GlobalPosition.y;
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        db = GetNode<DetectionBox>("DetectionBox");
        
        db.reaction = () => {
            int randomIndex = (int)(rnd.Randi() % 3);
            GD.Print(GetType().Name, ": Random Index Chosen ", randomIndex);
            Vector2 dest = Constants.USER_DESTINATION_VECTORS[randomIndex];
            db.direction = (dest - anchor.GlobalPosition).Normalized();
            if(idling){
                idling = false;
                GD.Print(GetType().Name, ": Swing Activated");
                anim.Play("swing");
                animTimer.Start();
            }
        };
        // Create and configure the timer
        animTimer = new Timer
        {
            WaitTime = animationDuration,
            OneShot = true
        };
        animTimer.Connect("timeout", this, nameof(OnAnimTimerTimeout));
        AddChild(animTimer);


    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(idling){
            walkAimlessly();
        }

    }

    private void walkAimlessly()
    {
        bool switchRight = Position.x <= Constants.STOP_LEFT;
        bool switchLeft = Position.x >= Constants.STOP_RIGHT;
        if (switchLeft || switchRight)
        {
            // flip sprite and change direction
            Scale = new Vector2(Scale.x * -1, Scale.y);
            direction *= -1;
        }

        bobUpAndDown();
        GlobalPosition = new Vector2(GlobalPosition.x + (direction * Constants.WALK_MAGNITUDE), GlobalPosition.y + (bounceDirection * Constants.BOUNCE_MAGNITUDE ));

    }

    private void walkTowardsBall(){

    }

    private void bobUpAndDown(){
        bool goDown = GlobalPosition.y > (initialHeight + Constants.BOUNCE_THRESHOLD);
        bool goUp = GlobalPosition.y <= (initialHeight - Constants.BOUNCE_THRESHOLD);

        if (goDown || goUp)
        {
            bounceDirection *= -1;
        }
    }

    // Method called when the animation timer times out
    private void OnAnimTimerTimeout()
    {
        anim.Stop(); // Stop the animation
        anim.Play("default"); // Play the default animation
        idling = true; // Set idling back to true
    }

}
