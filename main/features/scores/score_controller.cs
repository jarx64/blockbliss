
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class score_controller : Node
{
	 
	public signal on_score_updated;
	
	public GameStateHolder _game_state_holder 
	
	public void bind_services(GameStateHolder game_state_holder)
	{  
		_game_state_holder = game_state_holder;
		
	}
	
	public void setup()
	{  
		on_score_updated.emit();
	
	}
	
	public void add_score(int amount)
	{  
		_game_state_holder.game_state.score += amount;
		on_score_updated.emit(_game_state_holder.game_state.score);
	
	}
	
	public void reset_score()
	{  
		_game_state_holder.game_state.score = 0;
		on_score_updated.emit(_game_state_holder.game_state.score);
	
	
	}
	
	
	
}