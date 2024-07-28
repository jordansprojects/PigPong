using Godot;
using System;

public class DetectionBox : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	internal Action reaction;
	internal int magnitude = 55;
	internal Vector2 direction = Vector2.Down;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		// connect signal to detectBall
		 Connect("body_entered", this, "detectBall");
		
	}


    public void detectBall(Node2D body)
    {
        if (!(body is Ball))
        {
            return;
        }
		
        GD.Print(GetType().Name," : Ball detected" );
		Ball ball = (Ball)body;
		GD.Print(GetType().Name," : Ball size = ", ball.scalingFactor );
		if(ball.scalingFactor <= .85f * Constants.LARGEST_BALL ){
			GD.Print(GetType().Name," : Hitting ball" );
			reaction();
			ball.getHit(this.direction, this.magnitude);
		}
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
