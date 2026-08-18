
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class camera : Camera2D
{
	 
	public signal request_shake_screen;
	
	public const MAX_SHAKE: float = 5.0;
	
	public Vector2 home_position
	public Tween current_shake
	
	public void initialize()
	{  
		home_position = get_window().size / 2;
		position = home_position;
		request_shake_screen.connect(shake_screen);
		
	}
	
	public void shake_screen(int _row, Array _values[int])
	{  
		if(!current_shake || !current_shake.is_running())
		{
			current_shake = get_tree().create_tween();
			current_shake.tween_property(this, "position", home_position + new Vector2(randf_range(-MAX_SHAKE, MAX_SHAKE), randf_range(-MAX_SHAKE, MAX_SHAKE)), 0.05);
			current_shake.tween_property(this, "position", home_position + new Vector2(randf_range(-MAX_SHAKE, MAX_SHAKE), randf_range(-MAX_SHAKE, MAX_SHAKE)), 0.05);
			current_shake.tween_property(this, "position", home_position + new Vector2(randf_range(-MAX_SHAKE, MAX_SHAKE), randf_range(-MAX_SHAKE, MAX_SHAKE)), 0.05);
			current_shake.tween_property(this, "position", home_position + new Vector2(randf_range(-MAX_SHAKE, MAX_SHAKE), randf_range(-MAX_SHAKE, MAX_SHAKE)), 0.05);
			current_shake.tween_property(this, "position", home_position, 0.05);
	
		
	
	
		}
	}
	
	
	
}