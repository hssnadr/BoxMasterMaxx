using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    /// <summary>
    /// The canvas assigned to player 1.
    /// </summary>
    [SerializeField]
    protected Canvas player1Canvas;
    /// <summary>
    /// The canvas assigned to player 2.
    /// </summary>
    [SerializeField]
    protected Canvas player2Canvas;
    /// <summary>
    /// The prefab of the random target. (only used for testing)
    /// </summary>
    [SerializeField]
	protected MovementController targetMovementPrefab;
    /// <summary>
    /// Is the console mode activated ?
    /// </summary>
    [SerializeField]
    protected bool consoleMode;

    private MovementController _mc;

	private TargetController _targetP0;

	private TargetController _targetP1;
	private GameMode _gameMode;

	private int _soloIndex;

    private void OnEnable()
    {
        GameManager.onGameStart += OnGameStart;
        GameManager.onGameEnd += OnGameEnd;
        RandomTarget.onHit += OnHit;
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= OnGameStart;
        GameManager.onGameEnd -= OnGameEnd;
        RandomTarget.onHit -= OnHit;
    }

    private void Start()
    {
		OnGameStart(GameMode.P2, 0);
    }

    private Transform PlayerCanvas(int playerIndex)
    {
        if (playerIndex == 0)
            return player1Canvas.transform;
        else
            return player2Canvas.transform;
    }

    private void OnGameEnd()
    {
        _targetP0 = null;
        _targetP1 = null;
        _mc = null;
    }

	private void OnGameStart(GameMode gameMode, int soloIndex)
    {
		_gameMode = gameMode;
		_soloIndex = soloIndex;
		if (gameMode == GameMode.P1) {
			var go = GameObject.Instantiate (targetMovementPrefab);
            var controllers = go.GetComponentsInChildren<TargetController>();
            _mc = go;
            _targetP0 = controllers[0];
            _targetP0.playerIndex = GameManager.instance.soloIndex;
            _targetP0.Activate(1);
            _targetP1 = controllers[1];
            _targetP1.enabled = false;
		}
		else
		{
            int rand = Random.Range(0, 2);
			var go = GameObject.Instantiate(targetMovementPrefab);
			var controllers = go.GetComponentsInChildren<TargetController> ();
            _mc = go;
			_targetP0 = controllers [0];
			_targetP0.playerIndex = 0;
			_targetP0.Activate (rand);
			_targetP1 = controllers [1];
			_targetP1.playerIndex = 1;
			_targetP1.Activate (1 - rand);

			/*
			go0.playerIndex = 0;
			_targetP0 = go0;
			var go1 = GameObject.Instantiate(targetMovementPrefab);
			go1.playerIndex = 1;
			_targetP1 = go1;
			*/
		}
    }

	private void OnHit(int playerIndex)
    {
		if (_gameMode == GameMode.P1) {
            _targetP0.Activate ();
		} else if (_gameMode == GameMode.P2 && playerIndex == 0)
			_targetP1.Activate ();
        else if (_gameMode == GameMode.P2 && playerIndex == 1)
			_targetP0.Activate ();
    }
}
