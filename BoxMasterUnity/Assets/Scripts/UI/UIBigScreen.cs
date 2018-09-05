using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBigScreen : MonoBehaviour {
    /// <summary>
    /// The screen that displays the score.
    /// </summary>
    [SerializeField]
    [Tooltip("The screen that displays the score.")]
    private UIScoreScreen _scoreScreen = null;
    /// <summary>
    /// The page displayed for the countdown.
    /// </summary>
    [SerializeField]
    [Tooltip("The page displayed for the countdown.")]
    private UICountdownPage _countdownPage = null;
    /// <summary>
    /// The video displayed on the big screen.
    /// </summary>
    [SerializeField]
    [Tooltip("The video displayed on the big screen.")]
    private UIBigScreenVideo _video = null;
    /// <summary>
    /// The screen that display the final score.
    /// </summary>
    [SerializeField]
    [Tooltip("The screen that display the final score.")]
    private UIFinalScoreScreen _finalScoreScreen = null;

    private void OnEnable()
    {
        GameManager.onSetupEnd += OnSetupEnd;
        GameManager.onReturnToHome += OnReturnToHome;
        GameManager.onGameStart += OnGameStart;
        GameManager.onGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        GameManager.onSetupEnd -= OnSetupEnd;
        GameManager.onReturnToHome -= OnReturnToHome;
        GameManager.onGameStart -= OnGameStart;
        GameManager.onGameEnd -= OnGameEnd;
    }

    private void OnGameStart(GameMode gameMode, int soloIndex)
    {
        _countdownPage.Hide();
        _scoreScreen.Show();
        _finalScoreScreen.Hide();
        _video.Hide();
    }

    private void OnReturnToHome()
    {
        _countdownPage.Hide();
        _scoreScreen.Hide();
        _finalScoreScreen.Hide();
        _video.Show();
    }

    private void OnSetupEnd()
    {
        _countdownPage.Show();
        _scoreScreen.Hide();
        _finalScoreScreen.Hide();
        _video.Hide();
    }

    private void OnGameEnd()
    {
        _countdownPage.Hide();
        _scoreScreen.Hide();
        _finalScoreScreen.Show();
        _video.Hide();
    }
}
