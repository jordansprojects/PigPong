using Godot;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PlayPong")]
[assembly: InternalsVisibleTo("Wolf")]
[assembly: InternalsVisibleTo("Paddle")]
public class Ball : RigidBody2D
{
    internal float z = 1; 
    internal Vector2 anchor;
    private bool playersServe = true; 
    private bool isServed = false;
    private bool isHittable = false;
    private bool hadFirstHit = false;
    

    public override void _Ready(){

        
    }
  public override void _Process(float delta){
    if( ! isServed){
        awaitServe();
        GlobalPosition = anchor;
    }
    bounceOffTheCieling();
    
      
  }


    private void awaitServe(){
        if(playersServe){ // wait for spacebar input to serve ball
            if (Input.IsActionJustPressed("player_serve")){ // custom mapping for space bar
                beServed();
            }
        }

        if(isServed){
            simulateDistance();
            
        }
    }

    private void bounceOffTheCieling(){
        if(GlobalPosition.y < Constants.CIELING_Y){
            GD.Print("Bouncing off the cieling");
            ApplyCentralImpulse(Vector2.Down * Constants.BOUNCE_OFF_CIELING_MAGNITUDE);
        }
    }
    private void beServed(){
        GD.Print("Ball is being served");
        // apply impulse before ball can be hit by other objects
        isServed = true;
        hadFirstHit = false;
        ApplyCentralImpulse(Vector2.Up * Constants.SERVE_MAGNITUDE);
        isHittable = true;
       
    }

    internal void getHit(Vector2 impulseToApply, int mag){
        if (isHittable){
            GD.Print("Ball is Hit!");
            ApplyCentralImpulse(impulseToApply * mag);
            hadFirstHit = true;
        }
    }
    
    void simulateDistance(){
        z = GlobalPosition.y ;
        Scale = Scale * z;
    }


}
