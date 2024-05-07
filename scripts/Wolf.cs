using Godot;
using System;

public class Wolf : Node2D
{
    internal  int direction = 1;
    internal int bounceDirection = 1;
    private float initialHeight;
    private AnimatedSprite anim;
    private DetectionBox db;
  
    bool idling = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        initialHeight = GlobalPosition.y;
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        db = GetNode<DetectionBox>("DetectionBox");
        db.reaction = () => {
            GD.Print("Swing Activated");
        };

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(idling){
            walk();
        }

    }

    private void walk()
    {
        bool switchRight = Position.x <= Constants.STOP_LEFT;
        bool switchLeft = Position.x >= Constants.STOP_RIGHT;
        if (switchLeft || switchRight)
        {
            // flip sprite and change direction
            Scale = new Vector2(Scale.x * -1, Scale.y);
            direction *= -1;
        }

        bool goDown = GlobalPosition.y > (initialHeight + Constants.BOUNCE_THRESHOLD);
        bool goUp = GlobalPosition.y <= (initialHeight - Constants.BOUNCE_THRESHOLD);

        if (goDown || goUp)
        {
            bounceDirection *= -1;
        }

        GlobalPosition = new Vector2(GlobalPosition.x + (direction * Constants.WALK_MAGNITUDE), GlobalPosition.y + (bounceDirection * Constants.BOUNCE_MAGNITUDE ));

    }

}

internal class T
{
}