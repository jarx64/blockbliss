
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class grid_renderer : Node2D
{
	 
	public PackedScene tile_renderer_scene = GD.Load("res://features/grid/block_renderer.tscn");
	public PackedScene grid_cell_renderer_scene = GD.Load("res://features/grid/grid_cell_renderer.tscn");
	public PackedScene destroy_tile_renderer_particles = GD.Load("res://features/vfx/destroy_particles.tscn");
	
	Array rendered_tile_renderers[Sprite2D]
	Array dropping_tile_renderers[Sprite2D]
	
	public GridController _grid_controller
	public GameStateHolder _game_state_holder
	
	public GameState game_state:
		get: return _game_state_holder.game_state
	
	public void bind_services(GridController grid_controller, GameStateHolder gsh)
	{  
		_game_state_holder = gsh;
		_grid_controller = grid_controller;
	
	}
	
	public void initialize()
	{  
		position = new Vector2((get_window().size.x / 2.0) - (GameConstants.WIDTH * GameConstants.TILE_SIZE / 2.0), GameConstants.TILE_SIZE / 2.0);
		rendered_tile_renderers.resize(GameConstants.TILE_COUNT);
		draw_grid();
		
		_grid_controller.on_row_clear.connect(generate_clear_particles);
		
	}
	
	public void clear_grid()
	{  
		foreach(Node child in get_children())
		{
			child.queue_free();
		
		}
		foreach(Sprite2D tile_renderer in rendered_tile_renderers)
		{
			if(tile_renderer)
			{
				tile_renderer.queue_free();
		
			}
		}
		foreach(Sprite2D tile_renderer in dropping_tile_renderers)
		{
			if(tile_renderer)
			{
				tile_renderer.queue_free();
			}
		}
		dropping_tile_renderers = new Array(){};
	
	}
	
	public void draw_grid()
	{  
		clear_grid();
		foreach(int x in GD.Range(0, GameConstants.WIDTH))
		{
			foreach(int y in GD.Range(0, GameConstants.HEIGHT))
			{
				Node2D cell = grid_cell_renderer_scene.instantiate();
				add_child(cell);
				cell.position = Grid.address_to_position(Vector2i(x, y));
				
	
			}
		}
	}
	
	public void generate_clear_particles(int row, Array values[int])
	{  
		foreach(int i in GD.Range(GameConstants.WIDTH))
		{
			// render particles
			GPUParticles2D particles = destroy_tile_renderer_particles.instantiate();
			add_child(particles);
			particles.emitting = true;
			particles.position = Grid.index_to_position(row * GameConstants.WIDTH + i);
			particles.modulate = Grid.tile_to_color(values[i]);
			particles.finished.connect(particles.queue_free);
	
		}
	}
	
	public void update()
	{  
		// checks all rendered tiles; if a tile does !need to be changed, ignores it.
		// otherwise, removes, recolors, || creates it
		
		if(game_state.current_active_shape)
		{
			float percentage_of_next_row_dropped = (game_state.active_time - game_state.last_drop_time) / game_state.total_gravity ;
			bool grounded = _grid_controller.is_current_shape_touching_ground();
			
			// create new dropping tile_renderers
			if(dropping_tile_renderers.size() != game_state.current_active_shape.tiles.size())
			{
				foreach(Sprite2D dropping_tile_renderer in dropping_tile_renderers)
				{
					dropping_tile_renderer.queue_free();
				}
				dropping_tile_renderers = new Array(){};
				foreach(Vector2i tile in game_state.current_active_shape.tiles)
				{
					Sprite2D tile_renderer = tile_renderer_scene.instantiate();
					add_child(tile_renderer);
					dropping_tile_renderers.append(tile_renderer);
			
			// update dropping tile_renderers
				}
			}
			foreach(int tile_index in GD.Range(0, game_state.current_active_shape.tiles.size()))
			{
				Sprite2D dropping_tile_renderer = dropping_tile_renderers[tile_index];
				Color color = Grid.tile_to_color(game_state.current_active_shape.resource.color);
				
				if(dropping_tile_renderer)
				{
					dropping_tile_renderer.position = Grid.address_to_position(game_state.current_active_shape.tiles[tile_index])\
						+ new Vector2(0.0, grounded ? 0.0 : percentage_of_next_row_dropped * GameConstants.TILE_SIZE)
					dropping_tile_renderer.modulate = color;
	
		// TODO: optimize this by only covering changed indices, maybe?
				}
			}
		}
		foreach(int i in GD.Range(GameConstants.TILE_COUNT))
		{
			Sprite2D rendered_tile_renderer = rendered_tile_renderers[i];
			int value = game_state.grid[i];
			if(!rendered_tile_renderer && value > 0)
			{
				// create tile_renderer
				Sprite2D tile_renderer = tile_renderer_scene.instantiate();
				add_child(tile_renderer);
				rendered_tile_renderers[i] = tile_renderer;
				tile_renderer.position = Grid.index_to_position(i);
				tile_renderer.modulate = Grid.tile_to_color(value);
			}
			else if(rendered_tile_renderer && value ==  0)
			{
				// destroy tile_renderer
				rendered_tile_renderer.queue_free();
			}
			else if(rendered_tile_renderer && rendered_tile_renderer.modulate != Grid.tile_to_color(value))
			{
				// recolor tile_renderer
				rendered_tile_renderer.modulate = Grid.tile_to_color(value);
	
	
			}
		}
	}
	
	
	
}