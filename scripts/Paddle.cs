using Godot;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("PlayPong")]
public class Paddle : Area2D
{
    Vector2 direction = Vector2.Up;
    internal int magnitude = 55;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        // connect signal to paddle node
        Connect("body_entered", this, "detectBall");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }



    public void detectBall(Node2D body){
        if (body is Ball ){
            Ball ball = (Ball)body;
            GD.Print("Ball Detected! ");
            ball.getHit(this.direction, this.magnitude);
        }
    }

    internal void setDirection(Vector2 dir){
        direction = dir;
    }






}
