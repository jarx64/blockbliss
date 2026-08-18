
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class root_context : Node
{
	 
	@export PackedScene menu_scene_packed
	@export PackedScene game_scene_packed
	@export PackedScene music_player_packed
	@export PackedScene sfx_player_packed
	
	public SFXPlayer _sfx
	public MusicPlayer _music
	public GameStateHolder _game_state_holder
	
	public Node current_scene
	
	public void _ready()
	{  
		build_services();
		bind_services();
		go_to_menu();
	
	
	}
	
	public void build_services()
	{  
		_sfx = sfx_player_packed.instantiate() as SFXPlayer;
		add_child(_sfx);
		_music = music_player_packed.instantiate() as MusicPlayer;
		add_child(_music);
		_game_state_holder = new GameStateHolder()
		add_child(_game_state_holder);
		
	}
	
	public void bind_services()
	{  
		return;
	
	}
	
	public void go_to_menu()
	{  
		if(current_scene)
		{
			current_scene.queue_free();
		}
		current_scene = menu_scene_packed.instantiate();
		add_child(current_scene);
	
		MenuContext menu_scene = current_scene as MenuContext;
		if(menu_scene)
		{
			menu_scene.build_services();
			menu_scene.bind_services(_sfx, _music, _game_state_holder);
			menu_scene.connect_signals();
			menu_scene.setup();
			
			menu_scene.request_quit.connect(handle_request_quit);
			menu_scene.request_start_game.connect(handle_request_start_game);
			
		
		}
	}
	
	public void handle_request_start_game()
	{  
		if(current_scene)
		{
			current_scene.queue_free();
		}
		current_scene = game_scene_packed.instantiate();
		add_child(current_scene);
		
		// TODO: start playing game music!
		
		GameContext game_scene = current_scene as GameContext;
		if(game_scene)
		{
			game_scene.build_services();
			game_scene.bind_services(_sfx, _music, _game_state_holder);
			game_scene.handle_start_new_game();
			
			game_scene.on_game_loss.connect(handle_loss);
	
		}
	}
	
	public void handle_request_quit()
	{  
		get_tree().quit();
		
	}
	
	public void handle_loss()
	{  
		go_to_menu();
	
	
	}
	
	
	
}