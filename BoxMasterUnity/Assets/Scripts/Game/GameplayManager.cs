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
        OnGameStart();
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

    private void OnGameStart()
    {
        int playerIndex = GameManager.instance.gameMode == GameMode.P1 ? GameManager.instance.soloIndex : Random.Range(0, 2);
        var go = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(playerIndex));
        go.playerIndex = playerIndex;
    }

    private void OnHit(int playerIndex)
    {
        if (GameManager.instance.gameMode == GameMode.P1)
        {
            var go = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(playerIndex));
            go.playerIndex = playerIndex;
        }
        else if (GameManager.instance.gameMode == GameMode.P2 && playerIndex == 0)
        {
            var go = GameObject.Instantiate(randomTargetPrefab, player2Canvas.transform);
            go.playerIndex = 1;
        }
        else if (GameManager.instance.gameMode == GameMode.P2 && playerIndex == 1)
        {
            var go = GameObject.Instantiate(randomTargetPrefab, player1Canvas.transform);
            go.playerIndex = 0;
        }
    }
}
