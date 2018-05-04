using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public enum GameState {
	None,
	Home,
	Pages,
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
    public static event GameManagerEvent onGameStart;

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
	protected float _time1 = 0;
	[SerializeField]
	protected float _time2 = 0;
    [SerializeField]
    protected int _countdown = 0;

	protected bool _sleep = true;

	public GameState gameState {
		get {
			return _gameState;
		}
	}

	public float timeOut1 {
		get {
			return Time.time - _time1;
		}
	}

	public float timeOut2 {
		get {
			return Time.time - _time2;
		}
	}

    public int countdown
    {
        get {
            return _countdown;
        }
    }

    public int player1Score { get; private set; }
    public int player2Score { get; private set; }
    public int gameTime { get; private set; }
    public MainCamera player1Camera { get; private set; }
    public MainCamera player2Camera { get; private set; }

    public bool gameHasStarted
    {
        get { return _gameState == GameState.Game; }
    }
    public string gameSettingsPath = "init.xml";

	void Awake() {
		Init ();
	}

    void Start()
    {
        player1Camera = GameObject.FindGameObjectWithTag("Player1Camera").GetComponent<MainCamera>();
        player2Camera = GameObject.FindGameObjectWithTag("Player2Camera").GetComponent<MainCamera>();
    }

	void Init()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
			gameSettings = GameSettings.Load (Path.Combine (Application.dataPath, gameSettingsPath));
			_gameState = GameState.Home;
			_sleep = false;
		} else if (_instance != this) {
			Destroy (gameObject);
			Destroy (this);
		}
	}

	void Update()
	{
		if (Input.anyKeyDown && _gameState != GameState.Home) {
			_time1 = Time.time;
			_time2 = Time.time;
			_sleep = false;
			onActivity ();
		}
		if (Input.GetKeyUp (KeyCode.F11) || Input.GetMouseButtonUp(1)) {
			if (onReturnToOpening != null)
				onReturnToOpening();
		}
		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void Home()
	{
		_gameState = GameState.Home;
		StopAllCoroutines ();
	}

	public void StartPages()
	{
		_gameState = GameState.Pages;
		StartCoroutine (TimeOut());
	}

    public void StartGame()
    {
        _gameState = GameState.Game;
    }

	IEnumerator TimeOut()
	{
		bool timeOutScreenOn = false;
		_time1 = Time.time;
		_time2 = 0.0f;

		while (true) {
			yield return null;
			if (!_sleep) {
				if (timeOut1 >= gameSettings.timeOutScreen
					&& !timeOutScreenOn) {
					if (onTimeOutScreen != null)
						onTimeOutScreen ();
					timeOutScreenOn = true;
					_time2 = Time.time;
				} else if (timeOut1 <= gameSettings.timeOutScreen) {
					timeOutScreenOn = false;
				}
				if (timeOut2 >= gameSettings.timeOut && timeOutScreenOn) {
					if (onTimeOut != null)
						onTimeOut ();
					break;
				}
			}
		}
	}

	void OnDestroy() {
		_instance = null;
	}
    
    public MainCamera GetCamera(uint index)
    {
        if (index == 0)
            return Camera.main.GetComponent<MainCamera>();
        if (index == 1)
            return player1Camera;
        if (index == 2)
            return player2Camera;
        return null;
    }
}
