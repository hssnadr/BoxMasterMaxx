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
        var go = GameObject.Instantiate(randomTargetPrefab, player1Canvas.transform);
        go.playerIndex = 0;
    }

    private void OnGameEnd()
    {

    }

    private void OnGameStart()
    {

    }

    private void OnHit(int playerIndex)
    {
        if (playerIndex == 0)
        {
            var go = GameObject.Instantiate(randomTargetPrefab, player2Canvas.transform);
            go.playerIndex = 1;
        }
        if (playerIndex == 1)
        {
            var go = GameObject.Instantiate(randomTargetPrefab, player1Canvas.transform);
            go.playerIndex = 0;
        }
    }
}
