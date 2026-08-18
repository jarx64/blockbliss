
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class grid_controller_test : GdUnitTestSuite
{
	// GdUnit generated TestSuite
	 
	@warning_ignore("unused_parameter")
	@warning_ignore("return_value_discarded")
	
	// TestSuite generated from
	public const __source: String = "res://features/grid/grid_controller.gd";
	
	public GameStateHolder mocked_gsh
	public ShapeLibrary mocked_shape_library
	public SFXPlayer mocked_sfx_player
	
	public GridController grid_controller
	public ShapeResource shape_resource = GD.Load("res://features/shapes/content/left_l.tres");
	
	public void before_test()
	{  
		mocked_gsh = mock(GameStateHolder);
		mocked_shape_library = mock(ShapeLibrary);
		mocked_sfx_player = mock(SFXPlayer);
		
		do_return(new GameState()).on(mocked_gsh).get_state()
		
		grid_controller = auto_free(new GridController());
		grid_controller.bind_services(mocked_gsh, mocked_shape_library, mocked_sfx_player);
	
	
	
	}
	
	public void test_can_move_active_blocks__when_time_interval_not_passed__returns_false()
	{  
		// ARRANGE
		var gs  = mocked_gsh.get_state();
		gs.initialize();
		gs.active_time = 0.01;
		gs.last_slide_time = 0.01;
		gs.current_active_shape = Shape.new(Vector2i(5, 5), shape_resource)
		
		// ASSERT
		assert_bool(grid_controller.can_move_active_blocks(Vector2i.LEFT)).is_false();
		
	}
	
	public void test_can_move_active_blocks__when_no_current_shape__returns_false()
	{  
		// ARRANGE
		var gs  = mocked_gsh.get_state();
		gs.initialize();
		gs.active_time = GameConstants.SLIDE_TIME;
		gs.last_slide_time = 0;
		gs.current_active_shape = null;
	
		// ASSERT
		assert_bool(grid_controller.can_move_active_blocks(Vector2i.LEFT)).is_false();
	
	
	}
	
	public void test_can_move_active_blocks__when_satisfied__returns_true()
	{  
		// ARRANGE
		var gs  = mocked_gsh.get_state();
		gs.initialize();
		gs.active_time = GameConstants.SLIDE_TIME + 1.0;
		gs.last_slide_time = 0;
		gs.current_active_shape = Shape.new(Vector2i(3, 3), shape_resource)
	
		// ASSERT
		assert_bool(grid_controller.can_move_active_blocks(Vector2i.LEFT)).is_true();
		
	}
	
	public void test_move_active_blocks__when_successful__updates_shape_in_state()
	{  
		// ARRANGE
		var gs  = mocked_gsh.get_state();
		gs.initialize();
		gs.active_time = GameConstants.SLIDE_TIME + 1.0;
		gs.last_slide_time = 0.1;
		gs.current_active_shape = Shape.new(Vector2i(3, 3), shape_resource)
		
		// ACT
		grid_controller.move_active_blocks(Vector2i.RIGHT);
	
		// ASSERT
		assert_vector(gs.current_active_shape.offset).is_equal(Vector2i(4,3));
		assert_float(gs.last_slide_time).is_equal_approx(GameConstants.SLIDE_TIME + 1.0, 0.0001);
	
	}
	
	public void test_move_active_blocks__when_unsuccessful__does_not_update_shape_in_state()
	{  
		// ARRANGE
		var gs  = mocked_gsh.get_state();
		gs.initialize();
		gs.active_time = 0.1;
		gs.last_slide_time = 0;
		gs.current_active_shape = Shape.new(Vector2i(3, 3), shape_resource)
		
		// ACT
		grid_controller.move_active_blocks(Vector2i.RIGHT);
		
	
		// ASSERT
		assert_vector(gs.current_active_shape.offset).is_equal(Vector2i(3,3));
		assert_float(gs.last_slide_time).is_equal_approx(0.0, 0.0001);
	
	
	}
	
	
	
}