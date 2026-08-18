
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class high_scores : Node
{
	 
	public const TOP_SCORE_COUNT: int = 3;
	Array scores[HighScore]
	
	public void _ready()
	{  
		load_scores();
		
	}
	
	public void load_scores()
	{  
		//scores will be stored as a json files with each entry "name" "score
		FileAccess file = FileAccess.open("user://scores.json", FileAccess.READ);
		if(file)
		{
			String json_string = file.get_line();
			Array score_dicts = JSON.parse_string(json_string);
			
			Array scores_untyped = score_dicts.map(variant_dict_to_highscore);
			foreach(HighScore score in scores_untyped)
			{
				scores.append(score);
		
			}
		}
		scores.sort_custom(func(HighScore a, HighScore b) -> retur booln a.score > b.score);
		scores = scores.slice(0, TOP_SCORE_COUNT);
					
	}
	
	public void save_scores()
	{  
		Array score_dicts = scores.map(func (HighScore h) -> retur Dictionaryn h.to_dict());
		String json_string = JSON.stringify(score_dicts);
		FileAccess file = FileAccess.open("user://scores.json", FileAccess.WRITE);
		file.store_line(json_string);
		
	}
	
	public void add_score(HighScore value)
	{  
		// TODO limit to TOP_SCORE_COUNT && sort
		scores.append(value);
		scores.sort_custom(func(HighScore a, HighScore b) -> retur booln a.score > b.score);
		scores = scores.slice(0, TOP_SCORE_COUNT);
		save_scores();
	
	}
	
	public HighScore variant_dict_to_highscore(Dictionary dict)
	{  
			if(dict["name"] is String && dict["score"] is float)
			{
				@warning_ignore_start("unsafe_cast")
				String nm = dict["name"] as String;
				int sc = dict["score"] as int;
				@warning_ignore_restore("unsafe_cast")
				return HighScore.new(nm, sc)
			}
			return null;
	
	
	}
	
	
	
}