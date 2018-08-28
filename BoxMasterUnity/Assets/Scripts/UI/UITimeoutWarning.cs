using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeoutWarning : MonoBehaviour {
    /// <summary>
    /// Canvas of the text of the first timeout warning.
    /// </summary>
    [SerializeField]
    [Tooltip("Canvas of the text of the first timeout warning.")]
    private CanvasGroup _canvas1 = null;
    /// <summary>
    /// Canvas of the text of the second timeout warning.
    /// </summary>
    [SerializeField]
    [Tooltip("Canvas of the text of the second timeout warning.")]
    private CanvasGroup _canvas2 = null;

    private void Update()
    {
        bool setup = (GameManager.instance.gameState == GameState.Setup);
        _canvas1.alpha = setup ? 0.0f : 1.0f;
        _canvas2.alpha = setup ? 1.0f : 0.0f;

    }
}
