using Godot;
using System;

public class PlayPong : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta){
    /* use GlobalPosition so that placement does not rely on parent node */
	 Vector2 mousePos =   GetViewport().GetMousePosition();

     bool shouldFlip = (mousePos.x < GlobalPosition.x && Scale.x > 0 || mousePos.x > GlobalPosition.x && Scale.x < 0 );
     if(shouldFlip){        // flip if the cursor is on the side of the player that the player isnt facing
        Scale = new Vector2(Scale.x * -1, Scale.y);
     }

    // credits : https://gamefromscratch.com/gamedev-math-recipes-rotating-to-face-a-point/
     /*float angle = (float)(Math.Atan2(mousePos.y - GlobalPosition.y , mousePos.x - GlobalPosition.x));
     angle = (float)(angle * ( 180 / Math.PI ));
      

     if(angle < 0 ){
        angle = 360 - (-angle);
     }
     RotationDegrees = (90.0f + angle) ;
     */
  }


}
