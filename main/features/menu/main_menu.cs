
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class main_menu : PanelContainer
{
	 
	public signal request_start_game;
	public signal request_quit;
	public signal request_scores;
	
	@export Button start_button
	@export Button scores_button
	@export Button quit_button
	
	public MusicPlayer _music
	public SFXPlayer _sfx
		
	public void initialize(MusicPlayer music, SFXPlayer sfx)
	{  
		bind_services(music, sfx);
		bind_events();
		
	}
	
	public void bind_services(MusicPlayer music, SFXPlayer sfx)
	{  
		_music = music;
		_sfx = sfx	;
	
	}
	
	public void bind_events()
	{  
		start_button.pressed.connect(on_press_start);
		
		scores_button.pressed.connect(on_press_scores);
	
		quit_button.pressed.connect(on_press_quit);
	
	
	}
	
	public void on_press_start()
	{  
		_sfx.request_sound(SFXPlayer.Key.SELECT);
		request_start_game.emit();
	
	}
	
	public void on_press_scores()
	{  
		_sfx.request_sound(SFXPlayer.Key.SELECT);
		request_scores.emit();
		hide();
	
	}
	
	public void on_press_quit()
	{  
		_sfx.request_sound(SFXPlayer.Key.SELECT);
		request_quit.emit();
	
	}
	
	public void hide_menu()
	{  
		visible = false;
		
	}
	
	public void show_menu()
	{  
		visible = true;
	
	
	}
	
	
	
}