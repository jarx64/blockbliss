
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class sfx : AudioStreamPlayer2D
{
	 
	enum Key {
		SELECT,
		IMPACT,
		CLEAR
	}
	
	@export Array sounds[AudioStream]
	
	public void request_sound(SFXPlayer key.Key)
	{  
		stream = sounds[(int)(key)];
		pitch_scale = randf_range(0.8, 1.2);
		play();
		
	
	
	}
	
	
	
}