using Godot;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PlayPong")]
[assembly: InternalsVisibleTo("Wolf")]
[assembly: InternalsVisibleTo("Paddle")]
[assembly: InternalsVisibleTo("ScoreText")]

/*
    The Ball functions as both the physical ball and the GameManager 
 */
public class Ball : RigidBody2D{
    internal Vector2 anchor;
    private bool playersServe = true; 
    internal bool isServed, isHittable, hadFirstHit;
    bool bouncedOffCieling;
    internal float scalingFactor = Constants.LARGEST_BALL;
    internal static int []points = {0,0};
    private CollisionShape2D collider;
    private AnimatedSprite anim ;
    private ScoreText st; 

    public override void _Ready(){
        collider = GetNode<CollisionShape2D>("CollisionShape2D");
        anim = GetNode<AnimatedSprite >("AnimatedSprite");
        st = GetNode<ScoreText>("../ScoreText");
        isServed = isHittable = hadFirstHit = false;

        
    }
  public override void _Process(float delta){
    if( ! isServed){
        AwaitServe();
    }else if (hadFirstHit){
        SimulateDistance();
    }
    BounceOffTheCieling();
    CheckIfOfBounds();
    ScaleBall();
      
  }

    private void AwaitServe(){
        bouncedOffCieling = false;
        if(playersServe){ // wait for spacebar input to serve ball
            GlobalPosition = anchor;
            if (Input.IsActionJustPressed("player_serve")){ // custom mapping for space bar
                beServed();
            }
        }
    }

    private void BounceOffTheCieling(){
        if(GlobalPosition.y < Constants.CIELING_Y && !bouncedOffCieling ){
            ApplyCentralImpulse(Vector2.Down * Constants.BOUNCE_OFF_CIELING_MAGNITUDE);
            isHittable = true;
            bouncedOffCieling = true;
        }
    }
    private void beServed(){
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
            ApplyCentralImpulse(impulseToApply * mag );
            hadFirstHit = true;
        }
    }
    
    void SimulateDistance(){
        int startY = 300;
        int stopY = 20;
        if( GlobalPosition.y < startY && GlobalPosition.y > stopY){
        float distance = Math.Abs(GlobalPosition.y - startY) / startY;
        scalingFactor = Constants.LARGEST_BALL - distance * (Constants.LARGEST_BALL - Constants.TINIEST_BALL);
        }
    }

    private bool CheckIfOfBounds(){
        if(GlobalPosition.y > Constants.VIEWPORT_HEIGHT || GlobalPosition.y < Constants.VIEWPORT_MIN  || GlobalPosition.x < Constants.VIEWPORT_MIN  || GlobalPosition.x > Constants.VIEWPORT_WIDTH){
            isServed = false;
            scalingFactor = Constants.LARGEST_BALL;
            ScaleBall();

            if(GlobalPosition.y > Constants.VIEWPORT_HEIGHT){
                // Pigs get a point
                ++points[1]; 
            }
            if(GlobalPosition.y < Constants.VIEWPORT_MIN){
                // Wolf gets a point
                ++points[0];
            }
            st.updateText(points);
         return true;

        } 
        return false;
    }

    void ScaleBall(){
        anim.Scale = new Vector2(1,1) * scalingFactor ;
        collider.Scale = new Vector2(1,1) * scalingFactor ;
    }


 internal void DisableBall(){
      Hide();
      GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);

   }


   internal void EnableBall(){
      Show();
      GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
   }



} // end of Ball class
