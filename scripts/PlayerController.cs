using System;
using Godot;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: InternalsVisibleTo("Paddle")]
public class PlayerController : Node2D
{
   private Area2D paddle, purpleCenterPaddle, superCenterPaddle;
   private Ball ball;
   private int current = (int)Constants.Pigs.PURPLE;
   private int left, right;

   internal bool isCenter;
   RandomNumberGenerator rnd;

   AnimatedSprite pigCenterAnim, pigLeftAnim, pigRightAnim, paddleSprite;
   // Called when the node enters the scene tree for the first time.
   public override void _Ready(){
      rnd = new RandomNumberGenerator();
      rnd.Randomize();
      getNodeReferences();
      
      // set hit direction for purple pig's center paddle, and initial hit direction for super pig's center paddle
      (purpleCenterPaddle as Paddle)?.setDirection((Constants.CENTER_LANE - GlobalPosition).Normalized());
      (purpleCenterPaddle as Paddle).magnitude = 35;
      (superCenterPaddle as Paddle)?.setDirection((Constants.RIGHT_LANE - GlobalPosition).Normalized());
      (superCenterPaddle as Paddle).magnitude = 80;
      
      // set pig indicies 
      left = current + 2;
      right = current + 1;
      
      // ensure pigs begin with proper sprite selected
      changePigAnims(); 


   }

   //  // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(float delta)
   {
      controlPaddle();
      if(pigSelectHandler() == true){
         changePigAnims();
      }
   }
   private void getNodeReferences(){
      paddle = GetNode<Area2D>("Paddle");
      purpleCenterPaddle = GetNode<Area2D>("PurpleCenterPaddle");
      superCenterPaddle = GetNode<Area2D>("SuperCenterPaddle");
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
      isCenter = (mousePos.x < Constants.RIGHT_BOUNDARY) && (mousePos.x > Constants.LEFT_BOUNDARY);
      bool isNotExcludedFromCenter = (Constants.Pigs)current != Constants.Pigs.SKULL ;
      bool isForcedToCenter = (Constants.Pigs)current == Constants.Pigs.SUPER;
      if (isCenter && isNotExcludedFromCenter || isForcedToCenter) {
         disablePaddle(paddle);
         if (current == (int)Constants.Pigs.PURPLE){
            enablePaddle(purpleCenterPaddle);
         }else{
            enablePaddle(superCenterPaddle);
         }
         
      }
      else{
         enablePaddle(paddle);
         disablePaddle(superCenterPaddle);
         disablePaddle(purpleCenterPaddle);
         // flip if the cursor is on the side of the player that the player isnt facing
         bool shouldFlip = mousePos.x < GlobalPosition.x && Scale.x > 0 || mousePos.x > GlobalPosition.x && Scale.x < 0;
         if (shouldFlip){ 
            Scale = new Vector2(Scale.x * -1, Scale.y);
         }
         
         }
   }

   /*
      Sets direction of the hit based on which pig is chosen and where the mouse is.
   */
   internal void setHitDirection(){
      Vector2 centerPaddleDir = Constants.CENTER_LANE;
      Vector2 sidePaddleDir =  Constants.CENTER_LANE;
      Vector2 mousPos = GetViewport().GetMousePosition();
         bool isLeft = mousPos.x < Constants.RIGHT_BOUNDARY;
         switch( current ){
            case (int)Constants.Pigs.SKULL:
               GD.Print("Skull");
               (paddle as Paddle).magnitude = 65;
               if(isLeft){
                  sidePaddleDir = Constants.RIGHT_LANE;
               }else{
                  sidePaddleDir = Constants.LEFT_LANE;
               }
               break;
            case (int)Constants.Pigs.PURPLE:
                GD.Print("Purple");
               (paddle as Paddle).magnitude = 35;
               if(isLeft){
                     sidePaddleDir = Constants.LEFT_LANE;
               }else if (!isCenter && !isLeft) {
                     sidePaddleDir = Constants.RIGHT_LANE;
               }
               break;
            case (int)Constants.Pigs.SUPER:
            GD.Print("Super");
               float randomFloat = rnd.Randf();
               centerPaddleDir = ( randomFloat <  0.5 )? Constants.LEFT_LANE : Constants.RIGHT_LANE;
               (superCenterPaddle as Paddle).setDirection((centerPaddleDir- paddle.GlobalPosition).Normalized());
               GD.Print(GetType().Name , " : Direction Chosen : ", centerPaddleDir , "Random Float Generated : ", randomFloat );
               break;
         }

         (paddle as Paddle).setDirection((sidePaddleDir- paddle.GlobalPosition).Normalized());
      }
  



} // End of PlayPong class