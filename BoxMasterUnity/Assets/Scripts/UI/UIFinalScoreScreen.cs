using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFinalScoreScreen : UIScreen {
    /// <summary>
    /// The text of the final score.
    /// </summary>
    [SerializeField]
    [Tooltip("The text of the final score.")]
    private Text _finalScoreText = null;
    /// <summary>
    /// The text of the best score.
    /// </summary>
    [SerializeField]
    private Text _bestScoreText = null;
    /// <summary>
    /// The text of the ranking.
    /// </summary>
    [SerializeField]
    [Tooltip("The text of the ranking.")]
    private Text _rankingText = null;

    public override void Show()
    {
        base.Show();
        int score = GameManager.instance.playerScore;
        GameMode mode = GameManager.instance.gameMode;
        _finalScoreText.text = _finalScoreText.text.Replace("[Var]", score.ToString());
        _bestScoreText.text = _bestScoreText.text.Replace("[Var]", GameManager.instance.GetBestScore(mode).ToString());
        _rankingText.text = _rankingText.text.Replace("[Var]", GameManager.instance.rank.ToString());
    }
}
