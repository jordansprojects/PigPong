using Godot;
using System;

public class SceneLoad : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }


/***************
Usage:  Connect to button on_pressed signal to use to switch scenes via button press.
        In signal menu, Select "Advanced" and add a string parameter containing the name of the scene
@param string sceneStr : The name of the scene you want to change to
***************/
    private void ChangeScene(String sceneStr){
        string scenePath = "res://scenes/" + sceneStr;
        GD.Print("Loading path: " + scenePath);
        GetTree().ChangeScene(scenePath);
    }



}
