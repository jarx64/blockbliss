
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class grid_test : GdUnitTestSuite
{
	// GdUnit generated TestSuite
	 
	@warning_ignore("unused_parameter")
	@warning_ignore("return_value_discarded")
	
	// TestSuite generated from
	public const __source: String = "res://features/grid/grid.gd";
	
	
	public void test_index_to_position(int index, Vector2 expected, _test_parameters := new Array(){
		new Array(){0, Vector2.ZERO},
		new Array(){1, new Vector2(32, 0)},
		new Array(){2, new Vector2(64, 0)},
		new Array(){10, new Vector2(0, 32)},
		new Array(){11, new Vector2(32, 32)}
	})
	{  
		assert_vector(Grid.index_to_position(index)).is_equal(expected);
		
	}
	
	public void test_address_to_position(Vector2i address, Vector2 expected, _test_paramters := new Array(){
		new Array(){Vector2i.ZERO, Vector2.ZERO},
		new Array(){Vector2i(1, 0), new Vector2(32, 0)},
		new Array(){Vector2i(2, 0), new Vector2(64, 0)},
		new Array(){Vector2i(0, 1), new Vector2(0, 32)},
		new Array(){Vector2i(1, 1), new Vector2(32, 32)},
	})
	{  
		assert_vector(Grid.address_to_position(address)).is_equal(expected);
	
	}
	
	public void test_tile_to_color()
	{  
		assert_object(Grid.tile_to_color(0)).is_equal(Color.WHITE);
		assert_object(Grid.tile_to_color(1)).is_equal(Color.hex(0x7776BCFF));
		
	}
	
	public void test_get_index_below_index()
	{  
		assert_int(Grid.get_index_below_index(0)).is_equal(10);
		assert_int(Grid.get_index_below_index(1)).is_equal(11);
		
	}
	
	public void test_index_to_v2i(int index, Vector2i expected, _test_parameters := new Array(){
		new Array(){0, Vector2i.ZERO},
		new Array(){1, Vector2i(1, 0)},
		new Array(){10, Vector2i(0, 1)},
		new Array(){11, Vector2i(1, 1)},
	})
	{  
		assert_vector(Grid.index_to_v2i(index)).is_equal(expected);
		
	}
	
	public void test_v2i_to_index(Vector2i address, int expected, _test_parameters := new Array(){
		new Array(){Vector2i.ZERO, 0},
		new Array(){Vector2i(1, 0), 1},
		new Array(){Vector2i(0, 1), 10},
		new Array(){Vector2i(1, 1), 11},
	})
	{  
		assert_int(Grid.v2i_to_index(address)).is_equal(expected);
	
	
	}
	
	
	
}