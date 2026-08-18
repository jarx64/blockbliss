
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class high_scores_menu : PanelContainer
{
	 
	public signal request_dismiss;
	
	@export Button back_button
	@export Control scores_parent
	
	public HighScores _high_scores
	
	public void initialize(HighScores high_scores)
	{  
		bind_events();
		bind_services(high_scores);
		load_scores();
		hide();
	
	}
	
	public void bind_events()
	{  
		back_button.pressed.connect(handle_back_pressed);
		
	}
	
	public void bind_services(HighScores high_scores)
	{  
		_high_scores = high_scores;
		
	}
	
	public void show_menu()
	{  
		load_scores();
		show();
	
	
	}
	
	public void handle_back_pressed()
	{  
		visible = false;
		request_dismiss.emit();
	
	}
	
	public void load_scores()
	{  
		foreach(Node child in scores_parent.get_children())
		{
			child.queue_free();
		
		}
		foreach(HighScore score in _high_scores.scores)
		{
			Label score_label = new Label()
			score_label.text = score.name.rpad(12, ".") + "........" + GD.Str(score.score).lpad(10, "0");
			scores_parent.add_child(score_label);
	
	
		}
	}
	
	
	
}