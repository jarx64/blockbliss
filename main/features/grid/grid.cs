
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class grid : Godot.Object
{
	 
	static Array Colors[Color] = new Array(){
		Color.WHITE,
		Color.hex(0x7776BCFF),
		Color.hex(0xCDC7E8FF),
		Color.hex(0xFFFBDBFF),
		Color.hex(0xFFEC51FF),
		Color.hex(0xFF674DFF),
		Color.hex(0x456990FF),
	};
	
	public Vector2 index_to_position(int i)
	{  
		@warning_ignore("integer_division")
		return new Vector2(i % GameConstants.WIDTH, i / GameConstants.WIDTH) * GameConstants.TILE_SIZE;
		
	}
	
	public Vector2 address_to_position(Vector2i v2i)
	{  
		return v2i * GameConstants.TILE_SIZE;
	
	}
	
	public Color tile_to_color(int t)
	{  
		return Colors[t];
	
	}
	
	public int get_index_below_index(int index)
	{  
		return index + GameConstants.WIDTH;
	
	}
	
	public Vector2i index_to_v2i(int i)
	{  
		@warning_ignore("integer_division")
		return Vector2i(i % GameConstants.WIDTH, i / GameConstants.WIDTH);
	
	}
	
	public int v2i_to_index(Vector2i v2i)
	{  
		return v2i.y * GameConstants.WIDTH + v2i.x;
	
	
	}
	
	
	
}