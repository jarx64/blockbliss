
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class game_constants : Godot.Object
{
	 
	public static int HEIGHT = 20;
	public static int WIDTH = 10;
	public static int TILE_COUNT = HEIGHT * WIDTH;
	public static float TILE_DROP_SPEEDUP = 12.0 ;// how much faster tiles go if you're pressing down
	public static int TILE_SIZE = 32;
	public static float DIFFICULTY_RAMP = 25.0  ;// The higher it is, the slower the speed ramps up. Linear for now
	public static float SLIDE_TIME = 0.1 ;// Time between horizontal movements
	
	enum GAME_STATUS { NOT_STARTED, ACTIVE, PAUSED, FINISHED }
	enum TILE { NONE, RED, BLUE, YELLOW }
	
	
	
	
}