// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public enum GameState
{
    None,
    Home,
    Pages,
    Sleep,
    Game,
    End,
}

public class GameManager : MonoBehaviour
{
    public delegate void GameManagerEvent();
    public static event GameManagerEvent onTimeOutScreen;
    public static event GameManagerEvent onTimeOut;
    public static event GameManagerEvent onActivity;
    public static event GameManagerEvent onReturnToOpening;
    public static event GameManagerEvent onGameStart;

    public static GameManager instance
    {
        get
        {
            if (s_instance == null)
            {
                new GameObject("GameManager").AddComponent<GameManager>().Init();
            }
            return s_instance;
        }
    }

    private static GameManager s_instance = null;

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
    [SerializeField]
    protected float _gameTime = 0;

    protected bool _sleep = true;

    public GameState gameState
    {
        get
        {
            return _gameState;
        }
    }

    public float timeOut1
    {
        get
        {
            return Time.time - _time1;
        }
    }

    public float timeOut2
    {
        get
        {
            return Time.time - _time2;
        }
    }

    public float gameTime
    {
        get
        {
            return gameSettings.gameTime - (Time.time - _gameTime);
        }
    }

    public int countdown
    {
        get
        {
            return _countdown;
        }
    }

    public int player1Score { get; private set; }
    public int player2Score { get; private set; }
    public MainCamera player1Camera { get; private set; }
    public MainCamera player2Camera { get; private set; }
    /// <summary>
    /// The console text of the P1 display
    /// </summary>
    [SerializeField]
    protected Text _p1ConsoleText;
    /// <summary>
    /// The console text of the P2 display
    /// </summary>
    [SerializeField]
    protected Text _p2ConsoleText;

    public bool gameHasStarted
    {
        get { return _gameState == GameState.Game; }
    }
    public string gameSettingsPath = "init.xml";

    private void OnEnable()
    {
        ImpactPointControl.onImpact += OnImpact;
    }

    private void OnDisable()
    {
        ImpactPointControl.onImpact -= OnImpact;
    }

    private void OnImpact(Vector2 position, int playerIndex)
    {
        Debug.Log("IMPACT: " + position);
        Activity();
    }

    private void Activity()
    {
        if (gameState != GameState.Home)
        {
            _time1 = Time.time;
            _time2 = Time.time;
            _sleep = false;
            onActivity();
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        player1Camera = GameObject.FindGameObjectWithTag("Player1Camera").GetComponent<MainCamera>();
        player2Camera = GameObject.FindGameObjectWithTag("Player2Camera").GetComponent<MainCamera>();
    }

    private void Init()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            gameSettings = GameSettings.Load(Path.Combine(Application.dataPath, gameSettingsPath));
            _gameState = GameState.Home;
            _sleep = false;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Activity();
        }
        if (Input.GetKeyUp(KeyCode.F11) || Input.GetMouseButtonUp(1))
        {
            if (onReturnToOpening != null)
                onReturnToOpening();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Home()
    {
        _gameState = GameState.Home;
        StopAllCoroutines();
    }

    public void StartPages()
    {
        _gameState = GameState.Pages;
        StartCoroutine(TimeOut());
    }

    public void StartGame()
    {
        player1Score = 0;
        player2Score = 0;
        _gameState = GameState.Game;
        _gameTime = Time.time;
    }

    private IEnumerator TimeOut()
    {
        bool timeOutScreenOn = false;
        _time1 = Time.time;
        _time2 = 0.0f;

        while (true)
        {
            yield return null;
            if (!_sleep)
            {
                if (timeOut1 >= gameSettings.timeOutScreen
                    && !timeOutScreenOn)
                {
                    if (onTimeOutScreen != null)
                        onTimeOutScreen();
                    timeOutScreenOn = true;
                    _time2 = Time.time;
                }
                else if (timeOut1 <= gameSettings.timeOutScreen)
                {
                    timeOutScreenOn = false;
                }
                if (timeOut2 >= gameSettings.timeOut && timeOutScreenOn)
                {
                    if (onTimeOut != null)
                        onTimeOut();
                    break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public void ScoreUp(int playerIndex)
    {
        if (playerIndex == 0)
            player1Score++;
        if (playerIndex == 1)
            player2Score++;
    }

    public MainCamera GetCamera(int index)
    {
        if (index == 0)
            return player1Camera;
        if (index == 1)
            return player2Camera;
        if (index == 2)
            return Camera.main.GetComponent<MainCamera>();
        return null;
    }

    public Text GetConsoleText(int index)
    {
        if (index == 0)
            return _p1ConsoleText;
        if (index == 1)
            return _p2ConsoleText;
        return null;
    }
}
