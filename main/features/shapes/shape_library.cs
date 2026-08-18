
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class shape_library : Node
{
	 
	@export
	Array shapes[ShapeResource]
	
	public ShapeResource get_random_shape()
	{  
		return shapes.pick_random();
	
	
	}
	
	
	
}