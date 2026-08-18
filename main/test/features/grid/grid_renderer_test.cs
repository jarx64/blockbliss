
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class grid_renderer_test : GdUnitTestSuite
{
	// GdUnit generated TestSuite
	 
	@warning_ignore("unused_parameter")
	@warning_ignore("return_value_discarded")
	
	// TestSuite generated from
	public const __source: String = "res://features/grid/grid_renderer.gd";
	
	public GameStateHolder mocked_gsh
	public GridController mocked_grid_controller
	
	public GridRenderer grid_renderer
	
	public GdUnitSceneRunner runner
	
	public void before_test()
	{  	
		grid_renderer = auto_free(new GridRenderer());
		
		mocked_gsh = mock(GameStateHolder);
		mocked_grid_controller = mock(GridController);
		
		do_return(new GameState()).on(mocked_gsh).get_state()
	
		grid_renderer.bind_services(mocked_grid_controller, mocked_gsh);
	
	}
	
	public void test_draw_grid()
	{  	
		// ACT
		grid_renderer.draw_grid();
		
		// ASSERT
		assert_int(grid_renderer.get_child_count()).is_equal(GameConstants.TILE_COUNT);
	
	
	}
	
	
	
}