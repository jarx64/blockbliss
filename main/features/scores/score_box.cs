
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class score_box : PanelContainer
{
	 
	public ScoreController _score_controller
	
	@export 
	public Label score_display_label
	
	public void initialize(ScoreController score_controller)
	{  
		bind_services(score_controller);
		bind_events();
	
	}
	
	public void bind_services(ScoreController score_controller)
	{  
		_score_controller = score_controller;
		
	}
	
	public void bind_events()
	{  
		_score_controller.on_score_updated.connect(update_score);
	
	}
	
	public void update_score(int value)
	{  
		score_display_label.text = GD.Str(value).lpad(8, "0");
	
	
	}
	
	
	
}