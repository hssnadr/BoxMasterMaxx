using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
	Home,
	Game,

}

public class GameManager : MonoBehaviour {
	public static GameManager instance {
		get {
			if (_instance == null) {
				var instance = new GameObject ("GameManager").AddComponent<GameManager> ();
				instance.Init ();
			}
			return _instance;
		}
	}

	private static GameManager _instance = null;

	/// <summary>
	/// The game settings.
	/// </summary>
	public GameSettings gameSettings;

	public string gameSettingsPath = Application.dataPath + "init.xml";

	void Awake() {
		Init ();
	}

	void Init()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
			gameSettings = GameSettings.Load (gameSettingsPath);
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
