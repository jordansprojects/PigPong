using Godot;
using System;
using System.Diagnostics;
using System.Drawing;

public class PlayPong : KinematicBody2D
{
    private const int LEEWAY = 100;
    private const int CENTER_X  = 525;  
    private const int LEFT_BOUNDARY =  CENTER_X - LEEWAY;
    private const int RIGHT_BOUNDARY = CENTER_X + LEEWAY;
    private KinematicBody2D paddle;
    private AnimatedSprite paddleSprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        // store paddle and paddle collider so that it can be moved
        paddle =  GetNode<KinematicBody2D>("Paddle");
        paddleSprite = GetNode<AnimatedSprite>("Paddle/Paddlesprite");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta){
    /* use GlobalPosition so that placement does not rely on parent node */
	 Vector2 mousePos =   GetViewport().GetMousePosition();

    GD.Print("Mouse x : "  + mousePos.x );
    GD.Print("Right Boundary : " + RIGHT_BOUNDARY);
    GD.Print("Left Boundary : " + LEFT_BOUNDARY);

     bool isCenter = (mousePos.x < RIGHT_BOUNDARY) && (mousePos.x > LEFT_BOUNDARY);

      if (isCenter){
         GD.Print("Hiding");
         // paddleSprite.Hide();
         paddle.Hide();
      }else{
         paddle.Show();
     bool shouldFlip = (mousePos.x < GlobalPosition.x && Scale.x > 0 || mousePos.x > GlobalPosition.x && Scale.x < 0 );
     if(shouldFlip){        // flip if the cursor is on the side of the player that the player isnt facing
        Scale = new Vector2(Scale.x * -1, Scale.y);
     }
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
