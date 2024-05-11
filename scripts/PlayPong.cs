using System;
using Godot;

public class PlayPong : Node2D
{
   private Area2D paddle, centerPaddle;
   private Ball ball;
   private int current = (int)Constants.Pigs.PURPLE;
   private int left, right;

   AnimatedSprite pigCenterAnim, pigLeftAnim, pigRightAnim, paddleSprite;
   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      getNodeReferences();
      
      // set pig indicies 
      left = current + 2;
      right = current + 1;
      
      // ensure pigs begin with proper sprite selected
      changePigAnims(); 
      (centerPaddle as Paddle).setDirection((Constants.CENTER_LANE- GlobalPosition).Normalized());

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

      // first determine whether the paddle is in the center lane, this simplifies the options
      bool isCenter = (mousePos.x < Constants.RIGHT_BOUNDARY) && (mousePos.x > Constants.LEFT_BOUNDARY);
      bool isNotExcludedFromCenter = (Constants.Pigs)current != Constants.Pigs.SKULL ;
      bool isForcedToCenter = (Constants.Pigs)current == Constants.Pigs.SUPER;
      if (isCenter && isNotExcludedFromCenter || isForcedToCenter) {
         disablePaddle(paddle);
         enablePaddle(centerPaddle);
      }
      else{
         enablePaddle(paddle);
         disablePaddle(centerPaddle);
         // flip if the cursor is on the side of the player that the player isnt facing
         bool shouldFlip = mousePos.x < GlobalPosition.x && Scale.x > 0 || mousePos.x > GlobalPosition.x && Scale.x < 0;
         if (shouldFlip){ 
            Scale = new Vector2(Scale.x * -1, Scale.y);
         }
         configureHitDirection(mousePos, isCenter);
         }
   }

   /*
      controlPaddle helper function
   */
   private void configureHitDirection(Vector2 mousePos, bool isCenter){
      Vector2 hitDirection = Constants.CENTER_LANE;
         bool isLeft = mousePos.x < Constants.RIGHT_BOUNDARY;
         switch( current ){
            case (int)Constants.Pigs.SKULL:
               if(isLeft){
                  hitDirection = Constants.RIGHT_LANE;
               }else{
                  hitDirection = Constants.LEFT_LANE;
               }
               break;
            case (int)Constants.Pigs.PURPLE:
            if(isLeft){
                  hitDirection = Constants.LEFT_LANE;
            }else if (!isCenter && !isLeft) {
                  hitDirection = Constants.RIGHT_LANE;  
               }
               break;
            case (int)Constants.Pigs.SUPER:
            // Select random hit direction

            break;
         }
         (paddle as Paddle).setDirection((hitDirection - paddle.GlobalPosition).Normalized());
      }
      

   




   private void getNodeReferences(){
      paddle = GetNode<Area2D>("Paddle");
      centerPaddle = GetNode<Area2D>("CenterPaddle");
      pigLeftAnim = GetNode<AnimatedSprite>("../SeatedPigLeft");
      pigRightAnim = GetNode<AnimatedSprite>("../SeatedPigRight");
      pigCenterAnim = GetNode<AnimatedSprite>("Pigsprite");
      paddleSprite = GetNode<AnimatedSprite>("Paddle/PaddleSprite");
      ball = GetNode<RigidBody2D>("../Ball") as Ball;
   }

   private void disablePaddle(Area2D paddleObj){
      // Hide ball when disabling paddle to reduce glitching appearance when moving paddle & before a serve
      if(ball.isServed == false){ 
         ball.DisableBall();
      }
      paddleObj.Hide();
      paddleObj.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);

   }


   private void enablePaddle(Area2D paddleObj){
      paddleObj.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
      paddleObj.Show();

      ball.anchor = paddleObj.GlobalPosition + Constants.ANCHOR_OFFSET;
      ball.EnableBall();
   }


} // End of PlayPong class