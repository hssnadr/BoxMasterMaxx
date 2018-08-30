using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIScreen {
    [SerializeField]
    private Text _loadingText = null;

    private Coroutine _coroutine = null;
    public override void Show()
    {
        base.Show();
        if (_coroutine == null)
            _coroutine = StartCoroutine(LoadingTextRoutine());
    }

    private IEnumerator LoadingTextRoutine()
    {
        while (true)
        {
            _loadingText.text = "Loading";
            yield return new WaitForSeconds(0.25f);
            _loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.25f);
            _loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.25f);
            _loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
}
