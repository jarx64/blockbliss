
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class game_state_holder : Node
{
	 
	public GameState game_state
	
	public GameState get_state()
	{  
		return game_state;
	
	}
	
	public void clear()
	{  
		game_state = null;
	
	
	}
	
	
	
}