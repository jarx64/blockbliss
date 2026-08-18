
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class menu_overlay : Node
{
	 
	public signal on_dismiss_loss;
	public signal on_dismiss_highscores;
	
	@export MainMenu main_menu
	@export HighScoresMenu high_scores_menu
	@export EndScreen end_screen
	
	public MusicPlayer _music
	public GameStateHolder _game_state_holder
	
	public void initialize(MusicPlayer music,\
			sfx: SFXPlayer,\
			high_scores: HighScores,\
			gsh: GameStateHolder)
	{  
		
		_music = music;
		_game_state_holder = gsh;
				
		main_menu.initialize(music, sfx);
		high_scores_menu.initialize(high_scores);
		end_screen.initialize(high_scores, gsh);
		
		main_menu.request_scores.connect(high_scores_menu.show_menu);
		end_screen.request_dismiss_loss.connect(on_dismiss_loss.emit);
		on_dismiss_highscores.connect(handle_dismiss_highscores);
	
		update();
		
	}
	
	public void handle_dismiss_highscores()
	{  
		high_scores_menu.hide();
		main_menu.show_menu();
	
	}
	
	public void update()
	{  
		if(_game_state_holder.game_state && _game_state_holder.game_state.status == GameState.GAME_STATUS.LOST)
		{
			end_screen.show();
			main_menu.hide();
			high_scores_menu.hide();
		}
		else
		{
			end_screen.hide();
			main_menu.show();
			high_scores_menu.hide();
	
	
		}
	}
	
	
	
}