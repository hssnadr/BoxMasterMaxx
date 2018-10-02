// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRI.HitBox.Settings;

namespace CRI.HitBox.UI
{
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
        protected UICredits _copyrightScreen;
        /// <summary>
        /// The final score screen.
        /// </summary>
        [SerializeField]
        [Tooltip("The final score screen.")]
        protected UIFinalScoreScreen _finalScoreScreen;
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

        public bool loaded
        {
            get; private set;
        }

        private void OnEnable()
        {
            ApplicationManager.onActivity += OnActivity;
            ApplicationManager.onSetupEnd += OnSetupEnd;
            ApplicationManager.onTimeOutScreen += OnTimeOutScreen;
            ApplicationManager.onReturnToHome += OnReturnToHome;
            ApplicationManager.onStartPages += OnStartPages;
            ApplicationManager.onGameEnd += OnGameEnd;
        }

        private void OnDisable()
        {
            ApplicationManager.onActivity -= OnActivity;
            ApplicationManager.onSetupEnd -= OnSetupEnd;
            ApplicationManager.onTimeOutScreen -= OnTimeOutScreen;
            ApplicationManager.onReturnToHome -= OnReturnToHome;
            ApplicationManager.onGameEnd -= OnGameEnd;
        }

        private void OnGameEnd()
        {
            GoToFinalScoreScreen();
        }

        private void OnReturnToHome()
        {
            GoToHome();
        }

        private void OnStartPages()
        {
            GoToFirstPage();
        }

        private void OnSetupEnd()
        {
            GoToCountdownScreen();
        }

        private void OnTimeOutScreen()
        {
            GoToTimeoutScreen();
        }

        private void OnActivity()
        {
            if (_timeOutScreen.visible)
            {
                _timeOutScreen.Hide();
                _currentPage.Show();
            }
        }

        private IEnumerator Start()
        {
            loaded = false;
            while (!TextureManager.instance.isLoaded || !AudioManager.instance.isLoaded)
                yield return null;
            ScreenSettings[] screenSettingsArray = ApplicationManager.instance.menuSettings.pageSettings;

            _menuBar.Show();
            _pages = new IHideable[screenSettingsArray.Length];
            for (int i = 0; i < screenSettingsArray.Length; i++)
            {
                ScreenSettings screenSettings = screenSettingsArray[i];
                switch (screenSettings.GetScreenType())
                {
                    case ScreenSettings.ScreenType.ContentPage:
                        _pages[i] = InitContentPage((ContentPageSettings)screenSettings);
                        break;
                    case ScreenSettings.ScreenType.TextOnly:
                        _pages[i] = InitTextOnlyPage((TextOnlyPageSettings)screenSettings);
                        break;
                    case ScreenSettings.ScreenType.PlayerMode:
                        _pages[i] = InitChoosePlayerPage((PlayerModeSettings)screenSettings);
                        break;
                    case ScreenSettings.ScreenType.CatchScreen:
                        _pages[i] = InitCatchScreenPage((CatchScreenSettings)screenSettings);
                        break;
                    case ScreenSettings.ScreenType.Survey:
                        _pages[i] = InitSurveyPage((SurveyPageSettings)screenSettings);
                        break;
                }
            }
            _currentPage = _pages[0];
            _currentPage.Show();
            loaded = true;
        }

        private IHideable InitContentPage(ContentPageSettings screenSettings)
        {
            UIContentPage page = null;

            switch (screenSettings.contentScreenType)
            {
                case ContentPageSettings.ContentScreenType.Type1:
                    page = GameObject.Instantiate(_pagePrefabType1, _screens);
                    break;
                case ContentPageSettings.ContentScreenType.Type2:
                    page = GameObject.Instantiate(_pagePrefabType2, _screens);
                    break;
            }

            page.Init(screenSettings);

            return page;
        }

        private IHideable InitChoosePlayerPage(PlayerModeSettings screenSettings)
        {
            UIPlayerModePage page = null;

            page = GameObject.Instantiate(_pagePrefabTypeP1P2, _screens);

            page.Init(screenSettings);

            return page;
        }

        private IHideable InitTextOnlyPage(TextOnlyPageSettings screenSettings)
        {
            throw new NotImplementedException();
        }

        private IHideable InitCatchScreenPage(CatchScreenSettings screenSettings)
        {
            UICatchScreen page = null;

            page = GameObject.Instantiate(_catchScreenPrefab, _screens);

            page.Init(screenSettings);

            return page;
        }

        private IHideable InitSurveyPage(SurveyPageSettings screenSettings)
        {
            UISurveyScreen page = null;

            page = GameObject.Instantiate(_surveyScreenPrefab, _screens);

            page.Init(screenSettings);

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
        }

        public void GoToFirstPage()
        {
            AudioManager.instance.StopClip();
            GoTo(1);
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
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                _pageIndex = previousIndex;
                previous.Show();
                _currentPage = previous;
            }
            if (lastPage)
                ApplicationManager.instance.StartSetup();

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
            _currentPage.Hide();
            _countdownPage.Hide();
            _timeOutScreen.Hide();
            _scoreScreen.Show();
            _currentPage = _scoreScreen;
        }

        public void GoToFinalScoreScreen()
        {
            _currentPage.Hide();
            _timeOutScreen.Hide();
            _finalScoreScreen.Show();
            _currentPage = _finalScoreScreen;
        }

        public void GoToPage()
        {
            _currentPage.Hide();
            var previous = _currentPage;
            _currentPage = _pages[_pageIndex];
            previous.Hide();
            _currentPage.Show();
            _timeOutScreen.Hide();
        }

        public void GoToCredits()
        {
            _currentPage.Hide();
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
}