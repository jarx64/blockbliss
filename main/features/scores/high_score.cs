
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class high_score : Godot.Object
{
	 
	public String name 
	public int score
	
	public void _init(String nm, int scr)
	{  
		name = nm ;
		score = scr;
		
	}
	
	public Dictionary to_dict()
	{  
		Dictionary json_dict = new Dictionary(){{"name", name}, {"score", score}};
		return json_dict;
	
	
	}
	
	
	
}