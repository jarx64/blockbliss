
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class next_piece_renderer : PanelContainer
{
	 
	public signal on_new_next_shape;
	
	@export Control piece_frame
	@export Texture2D empty_cell_sprite
	@export Texture2D tile_sprite
	
	public const WIDTH: int = 5;
	public const HEIGHT: int = 5;
	public const OFFSET: int = 2;
	public const FRAME_OFFSET: Vector2 = new Vector2(16, 0);
	
	public void initialize()
	{  
		bind_listeners();
	
	}
	
	public void bind_listeners()
	{  
		on_new_next_shape.connect(render_next_piece);
	
	}
	
	public void render_next_piece(Shape shape)
	{  
		clear();
	
		if(!shape)
		{
			return;
	
		}
		foreach(int x in GD.Range(WIDTH))
		{
			foreach(int y in GD.Range(HEIGHT))
			{
					Sprite2D cell = new Sprite2D()
					piece_frame.add_child(cell);
					cell.position = (new Vector2(x, y) * GameConstants.TILE_SIZE) + new Vector2(0, 0.5 * GameConstants.TILE_SIZE) + FRAME_OFFSET;
					
					if(shape_has_tile_at_position(shape, x - OFFSET, y - OFFSET))
					{
						cell.texture = tile_sprite;
						cell.modulate = Grid.tile_to_color(shape.resource.color);
					}
					else
					{
						cell.texture = empty_cell_sprite;
	
					}
			}
		}
	}
	
	public bool shape_has_tile_at_position(Shape piece, int x, int y)
	{  
		return piece.resource.offsets.any(func(Vector2i t) -> retur booln t.x == x && t.y == y);
	
	}
	
	public void clear()
	{  
		foreach(Node child in piece_frame.get_children())
		{
			child.queue_free();
	
	
		}
	}
	
	
	
}