
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class end_screen : PanelContainer
{
	 
	public signal request_dismiss_loss;
	
	public const MAX_NAME_INPUT: int = 10;
	
	@export Button dismiss_button
	@export Label score
	@export LineEdit name_input
	
	public HighScores _high_scores
	public GameStateHolder _game_state_holder
	
	
	public void initialize(HighScores high_scores, GameStateHolder gsh)
	{  
		bind_services(high_scores, gsh);
		bind_events();
		hide();
	
	}
	
	public void bind_services(HighScores high_scores, GameStateHolder gsh)
	{  
		_high_scores = high_scores;
		_game_state_holder = gsh;
	
	}
	
	public void bind_events()
	{  	
		dismiss_button.pressed.connect(dismiss_loss);
		
	}
	
	public void handle_game_lost()
	{  
		visible = true;
		score.text = GD.Str(_game_state_holder.game_state.score);
	
	}
	
	public void dismiss_loss()
	{  
		visible = false;
		_high_scores.add_score(HighScore.new(name_input.text.substr(0, MAX_NAME_INPUT), _game_state_holder.game_state.score));
		request_dismiss_loss.emit();
	
	
	}
	
	
	
}