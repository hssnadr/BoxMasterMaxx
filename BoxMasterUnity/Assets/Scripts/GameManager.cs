using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
	None,
	Home,
	Game,
	End
}

public class GameManager : MonoBehaviour {
	public static GameManager instance {
		get {
			if (_instance == null) {
				new GameObject ("GameManager").AddComponent<GameManager> ().Init ();
			}
			return _instance;
		}
	}

	private static GameManager _instance = null;

	/// <summary>
	/// The game settings.
	/// </summary>
	public GameSettings gameSettings;

	/// <summary>
	/// The current state of the game
	/// </summary>
	[Tooltip("The current state of the game")]
	[SerializeField]
	private GameState _gameState = GameState.None;

	public GameState gameState {
		get {
			return _gameState;
		}
	}

	public string gameSettingsPath = "\\init.xml";

	void Awake() {
		Init ();
	}

	void Init()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
			gameSettings = GameSettings.Load (Application.dataPath + gameSettingsPath);
		} else if (_instance != this)
			Destroy (gameObject);
	}

	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Input.GetKeyUp (KeyCode.F11)) {
			// TODO return to opening screen
		}
	} 

	void OnDestroy() {
		_instance = null;
	}
}
