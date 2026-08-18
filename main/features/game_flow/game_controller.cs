
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class game_controller : Node
{
	 
	public signal on_game_loss;
	
	public ScoreController _score_controller
	public GridController _grid_controller
	public GameStateHolder _game_state_holder
	public GridRenderer _renderer
	public SFXPlayer _sfx
	public MusicPlayer _music
	
	public GameState state:
		get: return _game_state_holder.game_state
	
	
	public void bind_services(ScoreController score_controller,\
			grid_controller: GridController,\
			game_state_holder: GameStateHolder,\
			renderer: GridRenderer,
			sfx: SFXPlayer,
			music: MusicPlayer)
	{  
		_score_controller = score_controller;
		_grid_controller = grid_controller;
		_game_state_holder = game_state_holder;
		_renderer = renderer;
		_sfx = sfx;
		_music = music;
		
	
	}
	
	public void start()
	{  
		_score_controller.reset_score();
		state.status = GameState.GAME_STATUS.ACTIVE;
		_grid_controller.generate_new_active_tile_shape();
		_grid_controller.generate_new_active_tile_shape() ;// Generates a second so we have one in the queue
	
	}
	
	public void toggle_pause()
	{  
		if(state.status == GameState.GAME_STATUS.PAUSED)
		{
			state.status = state.last_status;
		}
		else
		{
			state.last_status = state.status;
			state.status = GameState.GAME_STATUS.PAUSED;
	
	
		}
	}
	
	public void update_game_state()
	{  
		// check if any grid tiles are in the top row && lose if so
		foreach(int i in GD.Range(GameConstants.WIDTH))
		{
			if(state.grid[i] != 0)
			{
				state.status = GameState.GAME_STATUS.LOST;
	
			}
		}
	}
	
	public void _process(float delta)
	{  
		if(state)
		{
			if(Input.is_action_just_pressed("pause"))
			{
				toggle_pause();
			
			}
			if(state.status == GameState.GAME_STATUS.ACTIVE)
			{
				state.active_time += delta;
				if(state.active_time - state.last_drop_time > state.total_gravity)
				{
					// drop current dropping tiles
					state.last_drop_time = state.active_time;
					if(state.current_active_shape)
					{
						if(_grid_controller.is_current_shape_touching_ground())
						{
							_sfx.request_sound(SFXPlayer.Key.IMPACT);
							_grid_controller.convert_active_tiles_to_grid();
							_grid_controller.check_and_clear_rows();
							update_game_state();
							_grid_controller.generate_new_active_tile_shape();
						}
						else
						{
							state.current_active_shape.offset += Vector2i(0, 1);
				
				// todo: allow holding down
						}
					}
				}
				if(Input.is_action_pressed("right"))
				{
					_grid_controller.move_active_blocks(Vector2i(1, 0));
				}
				else if(Input.is_action_pressed("left"))
				{
					_grid_controller.move_active_blocks(Vector2i(-1, 0));
	
				}
				if(Input.is_action_pressed("down"))
				{
					state.speed_up = true;
				}
				else
				{
					state.speed_up = false;
	
				}
				if(Input.is_action_just_pressed("up"))
				{
					_grid_controller.rotate_active_blocks();
	
				// TODO: Maybe clean up this dependency later?
				}
				_renderer.update();
			}
			else if(state.status == GameState.GAME_STATUS.LOST)
			{
				on_game_loss.emit();
			
	
	
			}
		}
	}
	
	
	
}