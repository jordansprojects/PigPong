using Godot;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
[assembly: InternalsVisibleTo("PlayPong")]
public class Paddle : Area2D
{
    Vector2 direction;
    internal int magnitude = 35;

    PlayPong playerController;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        // connect signal to paddle node
        Connect("body_entered", this, "detectBall");
        playerController = GetNode<PlayPong>("../../Pig");
    }

    public void detectBall(Node2D body)
    {
        if (!(body is Ball))
        {
            return;
        }

        Ball ball = (Ball)body;
        playerController.setHitDirection();

        ball.getHit(direction, magnitude);
        GD.Print(GetType().Name , ": Ball Detected. Hitting in the direction of : " + direction + " with a magninude of  "  + magnitude);
    }

    internal void setDirection(Vector2 dir){
        direction = dir;
    }






}
