
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class preview_renderer : Node2D
{
	 
	public PackedScene preview_renderer_tile = GD.Load("res://features/grid/preview_renderer.tscn");
	
	public GridController _grid_controller
	public GameStateHolder _game_state_holder
	
	public GameState game_state:
		get: return _game_state_holder.game_state
	
	public void _ready()
	{  
		position = new Vector2((get_window().size.x / 2.0) - (GameConstants.WIDTH * GameConstants.TILE_SIZE / 2.0), GameConstants.TILE_SIZE / 2.0);
	
	}
	
	public void _process(float _delta)
	{  
		clear();
		if(game_state && game_state.current_active_shape)
		{
			render();
	
		}
	}
	
	public void bind_services(GridController grid_controller, GameStateHolder gsh)
	{  
		_grid_controller = grid_controller;
		_game_state_holder = gsh;
	
	}
	
	public void render()
	{  
		// todo: maybe update movement rahter than clear && rerender for performance
		foreach(Vector2i tile_address in _grid_controller.get_drop_preview_tiles())
		{
			Sprite2D preview_tile = preview_renderer_tile.instantiate();
			add_child(preview_tile);
			preview_tile.position = Grid.address_to_position(tile_address);
	
		}
	}
	
	public void clear()
	{  
		foreach(Node child in get_children())
		{
			child.queue_free();
	
	
		}
	}
	
	
	
}