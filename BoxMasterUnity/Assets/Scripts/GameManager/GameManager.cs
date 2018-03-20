using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum GameState {
	None,
	Home,
	Sleep,
	Game,
	End,
}

public class GameManager : MonoBehaviour {
	public delegate void GameManagerEvent ();
	public static event GameManagerEvent onTimeOutScreen;
	public static event GameManagerEvent onTimeOut;
	public static event GameManagerEvent onActivity;
	public static event GameManagerEvent onReturnToOpening;

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

	[SerializeField]
	protected int _timeOutScreen = 0;
	[SerializeField]
	protected int _timeOut = 0;

	protected bool _sleep = true;

	public GameState gameState {
		get {
			return _gameState;
		}
	}

	public string gameSettingsPath = "init.xml";

	void Awake() {
		Init ();
	}

	void Start() 
	{
		StartCoroutine (TimeOut());
	}

	void Init()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
			gameSettings = GameSettings.Load (Path.Combine(Application.dataPath, gameSettingsPath));
		} else if (_instance != this)
			Destroy (gameObject);
	}

	void Update()
	{ 
		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Input.GetKeyUp (KeyCode.F11)) {
			if (onReturnToOpening != null)
				onReturnToOpening();
		}
		if (Input.anyKeyDown) {
			_timeOutScreen = gameSettings.timeOutScreen;
			_timeOut = gameSettings.timeOut;
			_sleep = false;
			onActivity ();
		}
	}

	public void Sleep()
	{
		_sleep = true;
	}

	IEnumerator TimeOut()
	{
		bool firstScreen = false;
		bool firstTimeOut = false;
		_timeOutScreen = gameSettings.timeOutScreen;
		_timeOut = gameSettings.timeOut;
		while (true) {
			yield return new WaitForSeconds (1.0f);
			if (!_sleep) {
				_timeOutScreen--;
				_timeOut--;
				if (_timeOutScreen <= 0 && _timeOut > 0 && !firstScreen) {
					if (onTimeOutScreen != null)
						onTimeOutScreen ();
					firstScreen = true;
				} else if (_timeOutScreen > 0) {
					firstScreen = false;
				}
				if (_timeOut <= 0 && !firstTimeOut) {
					onTimeOut ();
					firstTimeOut = true;
				} else if (_timeOut > 0) {
					firstTimeOut = false;
				}
			}
		}
	}

	void OnDestroy() {
		_instance = null;
	}
}
