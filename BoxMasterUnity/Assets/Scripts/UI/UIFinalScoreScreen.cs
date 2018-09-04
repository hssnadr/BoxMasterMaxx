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
    /// The text of the ranking.
    /// </summary>
    [SerializeField]
    [Tooltip("The text of the ranking.")]
    private Text _rankingText = null;

    public override void Show()
    {
        base.Show();
        _finalScoreText.text = _finalScoreText.text.Replace("[Var]", GameManager.instance.playerScore.ToString());
        _rankingText.text = _rankingText.text.Replace("[Var]", GameManager.instance.GetRank().ToString());
    }
}
