using Godot;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PlayPong")]
[assembly: InternalsVisibleTo("Ball")]
public class Constants{

   internal const int VIEWPORT_WIDTH = 1200;
   internal const int VIEWPORT_HEIGHT = 600;
   internal const int VIEWPORT_MIN = 0;


/* PIG CONSTANTS */
   const int LEEWAY = 100;
   internal const int CENTER_X = 525;
   internal const int LEFT_BOUNDARY = CENTER_X - LEEWAY;
   internal const int RIGHT_BOUNDARY = CENTER_X + LEEWAY;

   internal const int NUM_PIGS =  3;
   internal enum Pigs
   {
      PURPLE, // she hits straight consistently
      SUPER,   // can hit any row but only hits down the middle
      SKULL  // can only hit edges but slams it to the other side

   }
    internal static List<string> PIG_STRINGS = new List<string> { "Purple", "Super", "Skull" };


   internal const int LANE_DEST_Y = 210;
   internal const int LANE_DEST_START_X = 430;
   internal const int LANE_PADDING = 70;

   internal static Vector2 LEFT_LANE = new Vector2(LANE_DEST_START_X, LANE_DEST_Y);
   internal static Vector2  CENTER_LANE = new Vector2(LEFT_LANE.x + LANE_PADDING, LANE_DEST_Y);
   internal static Vector2 RIGHT_LANE = new Vector2(CENTER_LANE.x + LANE_PADDING, LANE_DEST_Y);
 
   /* WOLF CONSTANTS */
   const int BOUNDARY_BUFFER = 80;
   internal const int STOP_LEFT = LEFT_BOUNDARY + BOUNDARY_BUFFER;
   internal const int STOP_RIGHT = RIGHT_BOUNDARY - BOUNDARY_BUFFER;
   internal const float WALK_MAGNITUDE = 0.25f; 
   internal const float BOUNCE_MAGNITUDE = 0.5f;
   internal const int BOUNCE_THRESHOLD = 15;

   /* BALL CONSTANTS */
   internal const int SERVE_MAGNITUDE = 35;
   internal const int BOUNCE_OFF_CIELING_MAGNITUDE = 80;
   internal const int CIELING_Y = 30;
   // used in PlayPong.cs when setting anchor constants
   internal const int ANCHOR_OFFSET_Y = -25;
   internal static Vector2 ANCHOR_OFFSET = new Vector2(0, ANCHOR_OFFSET_Y);


   internal const float TINIEST_BALL = 0.10f;
   internal const float LARGEST_BALL = 0.5f;





}