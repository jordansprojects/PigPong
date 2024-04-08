using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("PlayPong")]
[assembly: InternalsVisibleTo("Wolf")]
[assembly: InternalsVisibleTo("Paddle")]
public class Ball : RigidBody2D
{
    internal float z = 1; 
    internal Vector2 anchor;
    private bool playersServe = true; 
    internal bool isServed, isHittable, hadFirstHit;
    private float prevY ;

    

    private CollisionShape2D collider;
    private AnimatedSprite anim ;

    public override void _Ready(){
        collider = GetNode<CollisionShape2D>("CollisionShape2D");
        anim = GetNode<AnimatedSprite >("AnimatedSprite");
        isServed = isHittable = hadFirstHit = false;

        
    }
  public override void _Process(float delta){
    if( ! isServed){
        awaitServe();
    }else if (hadFirstHit){
        simulateDistance();
    }
    bounceOffTheCieling();
    checkIfOfBounds();
      
  }


    private void awaitServe(){
        if(playersServe){ // wait for spacebar input to serve ball
            GlobalPosition = anchor;
            if (Input.IsActionJustPressed("player_serve")){ // custom mapping for space bar
                beServed();
            }
        }

  
    }

    private void bounceOffTheCieling(){
        if(GlobalPosition.y < Constants.CIELING_Y){
            GD.Print("Bouncing off the cieling");
            ApplyCentralImpulse(Vector2.Down * Constants.BOUNCE_OFF_CIELING_MAGNITUDE);
            isHittable = true;
        }
    }
    private void beServed(){
        GD.Print("Ball is being served");
        // apply impulse before ball can be hit by other objects
        isServed = true;
        isHittable = false;
        hadFirstHit = false;
        LinearVelocity = Vector2.Zero;
        ApplyCentralImpulse(Vector2.Up * Constants.SERVE_MAGNITUDE);


       
    }

    internal void getHit(Vector2 impulseToApply, int mag){
        if (isHittable){
            LinearVelocity = Vector2.Zero;
            GD.Print("Hitting ball in direction :" + impulseToApply);
            ApplyCentralImpulse(impulseToApply * mag );
            hadFirstHit = true;
        }
    }
    
    void simulateDistance(){
        int startY = 300;
        int stopY = 20;
        if( GlobalPosition.y < startY && GlobalPosition.y > stopY){
        float distance = (Math.Abs(GlobalPosition.y - startY)) / startY;
        float scalingFactor = Constants.LARGEST_BALL - distance * (Constants.LARGEST_BALL - Constants.TINIEST_BALL);
        scaleBall(scalingFactor);
        }
    }

    private bool checkIfOfBounds(){
        if(GlobalPosition.y > Constants.VIEWPORT_HEIGHT || GlobalPosition.y < Constants.VIEWPORT_MIN  || GlobalPosition.x < Constants.VIEWPORT_MIN  || GlobalPosition.x > Constants.VIEWPORT_WIDTH){
            isServed = false;
            scaleBall(Constants.LARGEST_BALL);
            return true;
        } 
        return false;
    }

    void scaleBall(float scalingFactor){
        anim.Scale = new Vector2(1,1) * scalingFactor ;
        collider.Scale = new Vector2(1,1) * scalingFactor ;
    }


 internal void disableBall(){
      Hide();
      GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);

   }


   internal void enableBall(){
      Show();
      GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
   }



} // end of Ball class
