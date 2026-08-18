
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class grid_controller : Node
{
	 
	public signal on_new_next_shape;
	public signal on_row_clear;
	public signal request_add_score;
	
	public GameStateHolder _game_state_holder
	public ShapeLibrary _shape_library
	public SFXPlayer _sfx
	
	public GameState state:
		get: return _game_state_holder.get_state()
		
	public void bind_services(GameStateHolder game_state_holder, ShapeLibrary shape_library, SFXPlayer sfx)
	{  
		_game_state_holder = game_state_holder;
		_shape_library = shape_library;
		_sfx = sfx;
	
	}
	
	public void move_active_blocks(Vector2i direction)
	{  
		if(can_move_active_blocks(direction))
		{
			state.current_active_shape.offset += direction;
			state.last_slide_time = state.active_time;
	
		}
	}
	
	public bool can_move_active_blocks(Vector2i direction)
	{  
		if(state.active_time <= state.last_slide_time + GameConstants.SLIDE_TIME)
		{
			return false;
		}
		if !state.current_active_shape || state.current_active_shape\
			base.get_tiles_with_rot_and_offset(state.current_active_shape.rotation, state.current_active_shape.offset + direction)\
			base.any(is_position_illegal):
			return false;
		return true;
	
	}
	
	public void rotate_active_blocks()
	{  
		if(can_rotate_active_blocks())
		{
			state.current_active_shape.rotate();
	
		}
	}
	
	public bool can_rotate_active_blocks()
	{  
		// TODO: this is !stopping us from rotating!
		if state.current_active_shape\
			base.get_tiles_with_rot_and_offset(state.current_active_shape.rotation + 1, state.current_active_shape.offset)\
			base.any(is_position_illegal):
			return false;
		return true;
		
	}
	
	public bool is_position_illegal(Vector2i tile)
	{  
		return tile.x >= GameConstants.WIDTH \
		|| tile.x < 0 \
		|| tile.y >= GameConstants.HEIGHT \
		|| tile.y < 0 \
		|| state.grid[Grid.v2i_to_index(tile)] > 0
	
	}
	
	public bool is_touching_ground(Vector2i tile)
	{  
		// return true if on the bottom row
		if(tile.y >= GameConstants.HEIGHT - 1)
		{
			return true;
		}
		int tile_index = Grid.v2i_to_index(tile);
		int below_index = Grid.get_index_below_index(tile_index);
		// return true if there's a tile under it
		return state.grid[below_index] != 0;
		
	}
	
	public bool is_current_shape_touching_ground()
	{  
		if(!state.current_active_shape)
		{
			return false;
		}
		return state.current_active_shape.tiles.any(is_touching_ground);
	
	}
	
	public Array[Vector2i] get_drop_preview_tiles()
	{  
		Array tiles[Vector2i] = state.current_active_shape.tiles;
		int drop_y = GameConstants.HEIGHT - 1;
	
		// find the value of y when the shape would first be grounded
		foreach(int y in GD.Range(0, GameConstants.HEIGHT))
		{
			foreach(Vector2i tile in tiles)
			{
				if(is_touching_ground(tile + Vector2i(0, y)))
				{
					drop_y = y;
					break;
				}
			}
			if(drop_y < GameConstants.HEIGHT - 1)
			{
				break;
				
			
			}
		}
		Array untyped = tiles.map(func(Vector2i t) -> retur Vector2in t + Vector2i(0, drop_y));
		Array typed_array[Vector2i]
		typed_array.assign(untyped);
		return typed_array;
	
	}
	
	public void convert_active_tiles_to_grid()
	{  
		foreach(Vector2i tile in state.current_active_shape.tiles)
		{
			int tile_index = Grid.v2i_to_index(tile);
			state.grid[tile_index] = state.current_active_shape.resource.color;
		}
		state.current_active_shape = null;
	
	}
	
	public void generate_new_active_tile_shape()
	{  
		state.current_active_shape = state.next_active_shape;
		ShapeResource shape = _shape_library.get_random_shape() as ShapeResource;
		if(shape)
		{
			state.next_active_shape = Shape.new(Vector2i(5, 0), shape)
			on_new_next_shape.emit(state.next_active_shape);
	
		}
	}
	
	public bool can_clear_row(int row)
	{  
		int start_index = row * GameConstants.WIDTH;
		return GD.Range(0, GameConstants.WIDTH).all(func(int i) -> retur booln state.grid[i + start_index] > 0);
	
	}
	
	public void check_and_clear_rows()
	{  
		// this should go from top down since the row clears can cause things to fall!
		foreach(int i in GD.Range(0, GameConstants.HEIGHT))
		{
			if(can_clear_row(i))
			{
				_sfx.request_sound(SFXPlayer.Key.CLEAR);
				clear_row(i);
	
			}
		}
	}
	
	public void clear_row(int row)
	{  
		Array old_row[int] = state.grid.slice(row * GameConstants.WIDTH, (row + 1) * GameConstants.WIDTH + 1);
		on_row_clear.emit(row, old_row);
	
		// make all above rows fall by slicing the values in the removed row out, 
		// then adding in a blank row at top
		Array fresh_row[int] = new Array(){};
		fresh_row.resize(GameConstants.WIDTH);
		fresh_row.fill(0);
		Array new_grid[int] = fresh_row + state.grid.slice(0, row * GameConstants.WIDTH) + state.grid.slice((row + 1) * GameConstants.WIDTH, GameConstants.TILE_COUNT + 1);
		state.grid = new Array(){};
		foreach(int value in new_grid)
		{
			state.grid.append(value);
		
		// add score
		}
		request_add_score.emit(100);
	
	
	}
	
	
	
}