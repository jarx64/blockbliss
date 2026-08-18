
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class music : AudioStreamPlayer2D
{
	 
	@export AudioStream menu_track
	@export AudioStream loss_track
	@export Array game_tracks[AudioStream]
	
	public void play_menu_track()
	{  
		if(finished.is_connected(play_game_track))
		{
			finished.disconnect(play_game_track);
			
		}
		if(!finished.is_connected(play_menu_track))
		{
			finished.connect(play_menu_track);
		
		}
		stream = menu_track;
		play();
	
	}
	
	public void play_loss_track()
	{  
		if(finished.is_connected(play_game_track))
		{
			finished.disconnect(play_game_track);
			
		}
		if(!finished.is_connected(play_menu_track))
		{
			finished.connect(play_menu_track);
	
		}
		stream = loss_track;
		play();
	
		
	}
	
	public void play_game_track()
	{  
		if(finished.is_connected(play_menu_track))
		{
			finished.disconnect(play_menu_track);
		
		}
		finished.connect(play_game_track);
		stream = game_tracks.pick_random();
		play();
		
	
	
	}
	
	
	
}