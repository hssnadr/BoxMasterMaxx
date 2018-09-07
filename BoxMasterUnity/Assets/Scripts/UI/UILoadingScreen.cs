using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIScreen {
    [SerializeField]
    private Text _loadingText = null;

    private UIScreenMenu _screenMenu = null;

    private Coroutine _coroutine = null;

    protected override void Start()
    {
        if (_screenMenu == null)
            _screenMenu = GameObject.FindObjectOfType<UIScreenMenu>();
        base.Start();
    }

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

    protected override void Update()
    {
        base.Update();
        if (!_screenMenu.loaded && !visible)
            Show();
        if (_screenMenu.loaded && visible)
            Hide();
    }

    public override void Hide()
    {
        base.Hide();
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
}
