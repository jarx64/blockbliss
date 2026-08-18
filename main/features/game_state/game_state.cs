
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class game_state : Godot.Object
{
	 
	enum GAME_STATUS { NOT_STARTED, ACTIVE, PAUSED, LOST }
	
	public GAME_STATUS status = GAME_STATUS.NOT_STARTED;
	public GAME_STATUS last_status = GAME_STATUS.NOT_STARTED;
	public float active_time
	public float last_drop_time
	public float last_slide_time
	public Shape current_active_shape
	public Shape next_active_shape
	public float gravity = 0.5 ;// seconds per tile drop
	public bool speed_up = false;
	public int score
	// grid is an array of tiles with 0 representing empty higher values representing colored blocks 
	Array grid[int]
	
	public float difficulty_coefficient:
		get: return 1 + (active_time / GameConstants.DIFFICULTY_RAMP) 
	
	public float total_gravity:
		get:
			if(speed_up)
			{
				return gravity / difficulty_coefficient / GameConstants.TILE_DROP_SPEEDUP;
			}
			return gravity / difficulty_coefficient;
			
	public void initialize()
	{  
		active_time = 0;
		last_drop_time = 0;
		current_active_shape = null;
		next_active_shape = null;
		grid.resize(GameConstants.TILE_COUNT);
		grid.fill(GameConstants.TILE.NONE);
	
	
	}
	
	
	
}