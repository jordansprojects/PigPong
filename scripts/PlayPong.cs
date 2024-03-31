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
         disablePaddle(paddle);
         enablePaddle(centerPaddle);
        
      }
      else
      {
         enablePaddle(paddle);
         disablePaddle(centerPaddle);
         // flip if the cursor is on the side of the player that the player isnt facing
         bool shouldFlip = mousePos.x < GlobalPosition.x && Scale.x > 0 || mousePos.x > GlobalPosition.x && Scale.x < 0;
         if (shouldFlip){ 
            Scale = new Vector2(Scale.x * -1, Scale.y);
         }
      }

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
      paddleObj.Hide();
      paddleObj.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
   }


   private void enablePaddle(Area2D paddleObj){
      paddleObj.Show();
      paddleObj.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
      ball.anchor = paddleObj.GlobalPosition + Constants.ANCHOR_OFFSET;
   }

} // End of PlayPong class