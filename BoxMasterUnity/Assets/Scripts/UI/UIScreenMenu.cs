// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenMenu : MonoBehaviour
{
    /// <summary>
    /// The page array.
    /// </summary>
	[SerializeField]
    protected IHideable[] _pages;

    /// <summary>
    /// The time out screen.
    /// </summary>
	[SerializeField]
    [Tooltip("The time out screen.")]
    protected UIScreen _timeOutScreen;
    /// <summary>
    /// The copyright screen.
    /// </summary>
    [SerializeField]
    [Tooltip("The copyright screen.")]
    protected UIScreen _copyrightScreen;
    /// <summary>
    /// The countdown screen
    /// </summary>
    [SerializeField]
    [Tooltip("The countdown screen.")]
    protected UICountdownPage _countdownPage;
    /// <summary>
    /// The score screen.
    /// </summary>
    [SerializeField]
    [Tooltip("The score screen.")]
    protected UIScoreScreen _scoreScreen;
    /// <summary>
    /// The menu bar
    /// </summary>
    [SerializeField]
    [Tooltip("The menu bar.")]
    protected UIMenuAnimator _menuBar;
    /// <summary>
    /// First model of a page prefab
    /// </summary>
	[SerializeField]
    [Tooltip("First type of page prefab.")]
    protected UIContentPage _pagePrefabType1;
    /// <summary>
    /// Second model of a page prefab
    /// </summary>
	[SerializeField]
    [Tooltip("Second type of page prefab.")]
    protected UIContentPage _pagePrefabType2;
    /// <summary>
    /// The page prefab of the P1P2 selection.
    /// </summary>
    [SerializeField]
    [Tooltip("The page prefab of the P1P2 selection.")]
    protected UIPlayerModePage _pagePrefabTypeP1P2;
    [SerializeField]
    protected UICatchScreen _catchScreenPrefab;
    [SerializeField]
    protected UISurveyScreen _surveyScreenPrefab;
    [SerializeField]
    protected Transform _screens;
    /// <summary>
    /// The current page
    /// </summary>
	protected IHideable _currentPage;

    protected int _pageIndex = 0;

    public bool lastPage
    {
        get
        {
            return _pageIndex + 1 >= _pages.Length;
        }
    }

    public bool catchScreen
    {
        get
        {
            return currentPage == _pages[0];
        }
    }

    public IHideable currentPage
    {
        get
        {
            return _currentPage;
        }
    }

    public UIMenuAnimator menuBar
    {
        get { return _menuBar; }
    }

    private void OnEnable()
    {
        GameManager.onActivity += OnActivity;
        GameManager.onSetupEnd += OnSetupEnd;
        GameManager.onTimeOut += OnTimeOut;
        GameManager.onTimeOutScreen += OnTimeOutScreen;
        GameManager.onReturnToHome += OnReturnToHome;
    }

    private void OnReturnToHome()
    {
        GoToHome();
        GameManager.instance.Home();
        _menuBar.SetState(false);
    }

    private void OnDisable()
    {
        GameManager.onActivity -= OnActivity;
        GameManager.onSetupEnd -= OnSetupEnd;
        GameManager.onTimeOut -= OnTimeOut;
        GameManager.onTimeOutScreen -= OnTimeOutScreen;
        GameManager.onReturnToHome -= OnReturnToHome;
    }

    private void OnSetupEnd()
    {
        GoToCountdownScreen();
    }

    private void OnTimeOutScreen()
    {
        GoToTimeoutScreen();
    }

    private void OnTimeOut()
    {
        OnReturnToHome();
    }

    private void OnActivity()
    {
        _timeOutScreen.Hide();
        _currentPage.Show();
    }

    private void Start()
    {
        PageSettings[] pageSettingsArray = GameManager.instance.gameSettings.pageSettings;

        _menuBar.Show();
        _pages = new IHideable[pageSettingsArray.Length];
        for (int i = 0; i < pageSettingsArray.Length; i++)
        {
            PageSettings pageSettings = pageSettingsArray[i];
            switch (pageSettings.GetPageType())
            {
                case PageSettings.PageType.ContentPage:
                    _pages[i] = InitContentPage((ContentPageSettings)pageSettings);
                    break;
                case PageSettings.PageType.TextOnly:
                    _pages[i] = InitTextOnlyPage((TextOnlyPageSettings)pageSettings);
                    break;
                case PageSettings.PageType.PlayerMode:
                    _pages[i] = InitChoosePlayerPage((PlayerModeSettings)pageSettings);
                    break;
                case PageSettings.PageType.CatchScreen:
                    _pages[i] = InitCatchScreenPage((CatchScreenPageSettings)pageSettings);
                    break;
                case PageSettings.PageType.Survey:
                    _pages[i] = InitSurveyPage((SurveyPageSettings)pageSettings);
                    break;
            }
        }
        Debug.Log(_pages.Length);
        _currentPage = _pages[0];
        _currentPage.Show();
    }

    private IHideable InitContentPage(ContentPageSettings pageSettings)
    {
        UIContentPage page = null;

        switch (pageSettings.contentPageType)
        {
            case ContentPageSettings.ContentPageType.Type1:
                page = GameObject.Instantiate(_pagePrefabType1, _screens);
                break;
            case ContentPageSettings.ContentPageType.Type2:
                page = GameObject.Instantiate(_pagePrefabType2, _screens);
                break;
        }

        page.title.InitTranslatedText(pageSettings.title.key, pageSettings.title.common);
        page.content.InitTranslatedText(pageSettings.content.key, pageSettings.content.common);
        page.displayNext = pageSettings.displayNext;
        if (pageSettings.imagePath != null && pageSettings.imagePath != "")
            page.rawImage.texture = Resources.Load<Texture>(pageSettings.imagePath);
        else
            page.rawImage.enabled = false;
        page.videoTexture.enabled = (pageSettings.videoPath != null && pageSettings.videoPath != "");
        if (page.videoTexture.enabled)
            page.videoClipPath = pageSettings.videoPath;
        page.audioClipPath = pageSettings.audioPath;

        return page;
    }

    private IHideable InitChoosePlayerPage(PlayerModeSettings pageSettings)
    {
        UIPlayerModePage page = null;

        page = GameObject.Instantiate(_pagePrefabTypeP1P2, _screens);

        page.title.InitTranslatedText(pageSettings.title.key, pageSettings.title.common);
        page.displayNext = pageSettings.displayNext;

        return page;
    }

    private IHideable InitTextOnlyPage(TextOnlyPageSettings pageSettings)
    {
        throw new NotImplementedException();
    }

    private IHideable InitCatchScreenPage(CatchScreenPageSettings pageSettings)
    {
        UICatchScreen page = null;

        page = GameObject.Instantiate(_catchScreenPrefab, _screens);

        return page;
    }

    private IHideable InitSurveyPage(SurveyPageSettings pageSettings)
    {
        UISurveyScreen page = null;

        page = GameObject.Instantiate(_surveyScreenPrefab, _screens);
        page.title.InitTranslatedText(pageSettings.title.key, pageSettings.title.common);
        return page;
    }

    public IHideable GetNextPage()
    {
        var tempPageIndex = _pageIndex + 1;
        return (tempPageIndex >= _pages.Length) ? null : _pages[tempPageIndex];
    }

    public IHideable GetCurrentPage()
    {
        return _currentPage;
    }

    public IHideable GetPreviousPage()
    {
        var tempPageIndex = _pageIndex - 1;
        return (tempPageIndex < 0) ? null : _pages[tempPageIndex];
    }

    public void GoToHome()
    {
        GoTo(0);
        TextManager.instance.SetDefaultLang();
        GameManager.instance.Home();
    }

    public void GoToFirstPage()
    {
        GoTo(1);
        GameManager.instance.StartPages();
    }

    public void GoToLastPage()
    {
        GoTo(_pages.Length - 1);
    }

    public void GoTo(int index)
    {
        var previous = _currentPage;
        int previousIndex = _pageIndex;
        try
        {
            _pageIndex = index;
            _currentPage = _pages[_pageIndex];
            previous.Hide();
            _currentPage.Show();
        }
        catch
        {
            _pageIndex = previousIndex;
            previous.Show();
            _currentPage = previous;
        }
        if (lastPage)
            GameManager.instance.StartSetup();

        _timeOutScreen.Hide();
        _menuBar.SetState(false);
    }

    public void GoToTimeoutScreen()
    {
        _currentPage.Hide();
        _timeOutScreen.Show();
    }

    public void GoToCountdownScreen()
    {
        _currentPage.Hide();
        _timeOutScreen.Hide();
        _countdownPage.Show();
        _currentPage = _countdownPage;
    }

    public void GoToScoreScreen()
    {
        _countdownPage.Hide();
        _timeOutScreen.Hide();
        _currentPage.Hide();
        _scoreScreen.Show();
        _currentPage = _scoreScreen;
    }

    public void GoToPage()
    {
        var previous = _currentPage;
        _currentPage = _pages[_pageIndex];
        previous.Hide();
        _currentPage.Show();
        _timeOutScreen.Hide();
    }

    public void GoToCredits()
    {
        _countdownPage.Hide();
        _timeOutScreen.Hide();
        _currentPage.Hide();
        _copyrightScreen.Show();
        _currentPage = _copyrightScreen;
        _menuBar.SetState(false);
    }

    public void GoToNext()
    {
        GoTo(_pageIndex + 1);
    }

    public void GoToPrevious()
    {
        GoTo(_pageIndex - 1);
    }
}