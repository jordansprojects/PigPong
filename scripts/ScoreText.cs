using Godot;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ball")]
public class ScoreText : Label
{
    public override void _Ready(){
        this.SetGlobalPosition( new Vector2(31, 25));
        updateText(Ball.points);  
    }

    internal void updateText(int []points){
        Text = "Pigs : " + points[0] + " Wolf:  " + points[1]; 
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
