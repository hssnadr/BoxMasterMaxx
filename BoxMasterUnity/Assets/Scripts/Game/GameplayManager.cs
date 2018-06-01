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
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= OnGameStart;
        GameManager.onGameEnd -= OnGameEnd;
    }

    private void OnGameStart()
    {
        var p1randomTarget = Instantiate(randomTargetPrefab, player1Canvas.transform);
        var p2randomTarget = Instantiate(randomTargetPrefab, player2Canvas.transform);
    }

    private void OnGameEnd()
    {

    }
}
