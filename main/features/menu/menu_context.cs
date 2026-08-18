
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class menu_context : Node
{
	 
	public signal request_quit;
	public signal request_start_game;
	public signal request_scores;
	
	@export MenuOverlay overlay
	
	@export PackedScene camera_packed
	
	public SFXPlayer _sfx
	public MusicPlayer _music
	public HighScores _high_scores
	public GameStateHolder _game_state_holder
	public GameCamera _camera
	
	public void build_services()
	{  
		_high_scores = new HighScores()
		add_child(_high_scores);
		
		_camera = camera_packed.instantiate();
		add_child(_camera);
	
	}
	
	public void bind_services(SFXPlayer sfx, MusicPlayer music, GameStateHolder gsh)
	{  
		_sfx = sfx;
		_music = music;
		_game_state_holder = gsh;
	
	}
	
	public void setup()
	{  
		overlay.initialize(_music, _sfx, _high_scores, _game_state_holder)		;
		_camera.initialize();
	
		
		if(_game_state_holder.game_state &&  _game_state_holder.game_state.status == GameState.GAME_STATUS.LOST)
		{
			_music.play_loss_track();
		}
		else
		{
			_music.play_menu_track();
	
		}
	}
	
	public void connect_signals()
	{  
		overlay.main_menu.request_start_game.connect(request_start_game.emit);
		overlay.main_menu.request_quit.connect(request_quit.emit);
		overlay.on_dismiss_loss.connect(handle_dismiss_loss);
		
		overlay.high_scores_menu.request_dismiss.connect(overlay.on_dismiss_highscores.emit);
		
	
		
	}
	
	public void handle_dismiss_loss()
	{  
		_game_state_holder.clear();
		_music.play_menu_track();
		overlay.update();
		
	
	
	}
	
	
	
}