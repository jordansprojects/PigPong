using Godot;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;

public class PlayPong : Node2D
{
   private KinematicBody2D paddle;
   private KinematicBody2D centerPaddle;

   private int current = (int)Constants.Pigs.PURPLE;
   private int left, right;

   AnimatedSprite pigCenterAnim, pigLeftAnim, pigRightAnim, paddleSprite;
   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      getNodeReferences();
      left = current + 2;
      right = current + 1;

   }

   //  // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(float delta)
   {
      controlPaddle();
      if(pigSelectHandler() == true){
         changePigAnims();
      }
   }

   private void changePigAnims(){
      pigLeftAnim.Play(Constants.PIG_STRINGS[left]);
      pigRightAnim.Play(Constants.PIG_STRINGS[right]);
      pigCenterAnim.Play(Constants.PIG_STRINGS[current]);
      paddleSprite.Play(Constants.PIG_STRINGS[current]);
      paddleSprite.Stop(); // Comment this out if you think the paddle animation on loop is cute
   }

   private bool pigSelectHandler(){
      bool leftPressed = Input.IsActionJustPressed("ui_left");
      bool rightPressed = Input.IsActionJustPressed("ui_right");
      if (leftPressed || rightPressed){
         int shiftAmount = 0;
         if (leftPressed) {
            shiftAmount = 2;
         }
         if (rightPressed){
            shiftAmount = 1;
         }
         current = (current + shiftAmount) % Constants.NUM_PIGS;
         left = (left + shiftAmount) % Constants.NUM_PIGS;
         right = (right + shiftAmount) % Constants.NUM_PIGS;
         return true;
      }
      return false;
   }

   private void controlPaddle(){
      /* use GlobalPosition so that placement does not rely on parent node */
      Vector2 mousePos = GetViewport().GetMousePosition();
      bool isCenter = (mousePos.x < Constants.RIGHT_BOUNDARY) && (mousePos.x > Constants.LEFT_BOUNDARY);

      if (isCenter)
      {
         paddle.Hide();
         centerPaddle.Show();
      }
      else
      {
         paddle.Show();
         centerPaddle.Hide();
         bool shouldFlip = mousePos.x < GlobalPosition.x && Scale.x > 0 || mousePos.x > GlobalPosition.x && Scale.x < 0;

         if (shouldFlip)
         {        // flip if the cursor is on the side of the player that the player isnt facing
            Scale = new Vector2(Scale.x * -1, Scale.y);
         }
      }

   }

   private void getNodeReferences(){
      paddle = GetNode<KinematicBody2D>("Paddle");
      centerPaddle = GetNode<KinematicBody2D>("CenterPaddle");
      pigLeftAnim = GetNode<AnimatedSprite>("../SeatedPigLeft");
      pigRightAnim = GetNode<AnimatedSprite>("../SeatedPigRight");
      pigCenterAnim = GetNode<AnimatedSprite>("Pigsprite");
      paddleSprite = GetNode<AnimatedSprite>("Paddle/PaddleSprite");

   }

} // End of PlayPong class