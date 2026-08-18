
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class backdrop : Sprite2D
{
	 
	@export Array colors[Color]
	@export float transition_time
	@export float transition_time_fudge
	
	public Tween current_tween
	
	public void _ready()
	{  
		tween_random();
	
	
	}
	
	public void tween_random()
	{  
		current_tween = get_tree().create_tween();
		current_tween.tween_property(this, "modulate", colors.pick_random(),\
			randf_range(transition_time - transition_time_fudge, transition_time + transition_time_fudge));
		current_tween.tween_callback(tween_random);
	
	
	}
	
	
	
}