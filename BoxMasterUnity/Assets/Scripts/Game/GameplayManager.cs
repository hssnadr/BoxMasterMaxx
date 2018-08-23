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
    protected RandomTarget randomTargetPrefab;
    /// <summary>
    /// Is the console mode activated ?
    /// </summary>
    [SerializeField]
    protected bool consoleMode;

	private RandomTarget[] _targetP1 = new RandomTarget[2];

	private RandomTarget[] _targetP2 = new RandomTarget[2];

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

    }

	private void OnGameStart(GameMode gameMode, int soloIndex)
    {
		_gameMode = gameMode;
		_soloIndex = soloIndex;
		if (gameMode == GameMode.P1) {
			var go = GameObject.Instantiate (randomTargetPrefab, PlayerCanvas (soloIndex));
			go.playerIndex = soloIndex;
		}
		else
		{
			var go00 = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(0));
			go00.playerIndex = 0;
			_targetP1[0] = go00;
			var go01 = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(0));
			go01.playerIndex = 0;
			_targetP1 [1] = go01;
			var go10 = GameObject.Instantiate (randomTargetPrefab, PlayerCanvas (1));
			go10.playerIndex = 1;
			_targetP2 [0] = go10;
			var go11 = GameObject.Instantiate (randomTargetPrefab, PlayerCanvas (1));
			go11.playerIndex = 1;
			_targetP2 [1] = go11;

			bool rand = (Random.Range (0, 2) == 0);
			_targetP1 [0].activated = rand;
			_targetP1 [1].activated = false;
			_targetP2 [0].activated = !rand;
			_targetP2 [1].activated = false;
		}
    }

    private void OnHit(int playerIndex)
    {
        if (_gameMode == GameMode.P1)
        {
            var go = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(playerIndex));
            go.playerIndex = playerIndex;
        }
		else if (_gameMode == GameMode.P2 && playerIndex == 0)
        {
			int rand = Random.Range (0, 2);
			_targetP1 [0].activated = false;
			_targetP1 [1].activated = false;
			_targetP2 [rand].activated = true;
			_targetP2 [rand == 0 ? 1 : 0].activated = false;
        }
        else if (_gameMode == GameMode.P2 && playerIndex == 1)
        {

			int rand = Random.Range (0, 2);
			_targetP2 [0].activated = false;
			_targetP2 [1].activated = false;
			_targetP1 [rand].activated = true;
			_targetP1 [rand == 0 ? 1 : 0].activated = false;
        }
    }
}
