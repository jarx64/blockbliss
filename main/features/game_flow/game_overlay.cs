
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class game_overlay : Node
{
	 
	@export ScoreBox score_box
	@export NextPieceRenderer next_piece_renderer 
	
	public void initialize(ScoreController score_controller,\
			grid_controller: GridController)
	{  
				
		score_box.initialize(score_controller);
		
		grid_controller.on_new_next_shape.connect(next_piece_renderer.on_new_next_shape.emit);
		next_piece_renderer.initialize();
	
	
	}
	
	
	
}