
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class game_context : Node
{
	 
	public signal on_game_loss;
	
	// LOCAL STRUCTURE
	@export GridRenderer renderer
	@export PreviewRenderer preview_renderer
	@export GameOverlay overlay
	
	// PACKED SCENES
	@export PackedScene shape_library_packed
	@export PackedScene camera_packed
	
	
	public GameController _game_controller
	public GridController _grid_controller
	public ScoreController _score_controller
	public SFXPlayer _sfx
	public MusicPlayer _music
	public ShapeLibrary _shape_lib
	public GameStateHolder _game_state_holder
	public GameCamera _camera
	
	public void build_services()
	{  
		_game_controller = new GameController()
		add_child(_game_controller);
		_grid_controller = new GridController()
		add_child(_grid_controller);
		_score_controller = new ScoreController()
		add_child(_score_controller);
		_shape_lib = shape_library_packed.instantiate() as ShapeLibrary;
		add_child(_shape_lib);
		
		_camera = camera_packed.instantiate();
		add_child(_camera);
		
	
	}
	
	public void bind_services(SFXPlayer sfx, MusicPlayer music, GameStateHolder gsh)
	{  
		_sfx = sfx;
		_music = music;
		_game_state_holder = gsh;
	
		_game_controller.bind_services(_score_controller, _grid_controller, gsh, renderer, _sfx, _music);
		_grid_controller.bind_services(gsh, _shape_lib, _sfx);
		_score_controller.bind_services(gsh);
		_camera.initialize();
		
		_grid_controller.on_row_clear.connect(_camera.request_shake_screen.emit);
		_game_controller.on_game_loss.connect(on_game_loss.emit);
		_grid_controller.request_add_score.connect(_score_controller.add_score);
		
		renderer.bind_services(_grid_controller, _game_state_holder);
		preview_renderer.bind_services(_grid_controller, _game_state_holder);
		overlay.initialize(_score_controller, _grid_controller);
			
	
	}
	
	public void handle_start_new_game()
	{  
		_game_state_holder.game_state = new GameState()
		_game_state_holder.game_state.initialize();
		_game_controller.start();
	
		renderer.initialize();
		renderer.update();
		
		_score_controller.reset_score();
		_music.play_game_track();
		
		
	
	
	}
	
	
	
}