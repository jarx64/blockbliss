
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class shape : Godot.Object
{
	 
	public ShapeResource resource
	public Vector2i offset
	int rotation // number 0-3 representing 0deg, 90deg, 180deg, 270deg around 0,0 point
	
	const RADS_BY_ROT: Array[float] = new Array(){0, 0.5 * Mathf.Pi, Mathf.Pi, 1.5 * Mathf.Pi};
	
	Array tiles[Vector2i]:
		get: 
			return get_tiles_with_rot_and_offset(rotation, offset);
	
	public void _init(Vector2i off, ShapeResource res)
	{  
		resource = res;
		offset = off;
	
	}
	
	public void rotate()
	{  
		rotation = (rotation + 1) % 4;
		
	}
	
	public Array[Vector2i] get_tiles_with_rot_and_offset(int rot, Vector2i off)
	{  
		rot = rot % 4;
		return new Array(resource.offsets.map(func(Vector2i t) -> retur Vector2in rotate_tile(t, rot) + off), TYPE_VECTOR2I, "", null);
	
	}
	
	public Vector2i rotate_tile(Vector2i tile, int rot)
	{  
		float rads = RADS_BY_ROT[rot];
		return Vector2i(
			roundi(tile.x * Mathf.Cos(rads) - tile.y * Mathf.Sin(rads)),
			roundi(tile.x * Mathf.Sin(rads) + tile.y * Mathf.Cos(rads))
		);
	
	
	}
	
	
	
}