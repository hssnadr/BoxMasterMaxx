// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CRI.HitBox.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIMenuAnimator : MonoBehaviour, IHideable
    {
        [SerializeField]
        protected Animator _animator;
        [SerializeField]
        protected bool _open = false;
        [SerializeField]
        protected CanvasGroup _canvasGroup;
        [SerializeField]
        private CanvasGroup _maskCanvasGroup = null;
        [SerializeField]
        protected UILangMenu _UILangMenu;
        [SerializeField]
        protected GameObject _previousButton;
        [SerializeField]
        protected GameObject _nextButton;
        [SerializeField]
        protected GameObject _nextButton2;
        [SerializeField]
        protected GameObject _nextButton3;
        [SerializeField]
        protected GameObject _text;

        protected UIScreenMenu _screenMenu;

        protected Coroutine _closeMenuRoutine = null;

        private void OnEnable()
        {
            GameManager.onActivity += OnActivity;
        }

        private void OnActivity()
        {
            if (_open)
                CloseMenuOnCooldown();
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _UILangMenu = GetComponentInChildren<UILangMenu>();
            _screenMenu = GetComponentInParent<UIScreenMenu>();
        }

        public void ToggleAnimation()
        {
            SetState(!_open);
        }

        #region IHideable implementation

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            if (_closeMenuRoutine != null)
                StopCoroutine(_closeMenuRoutine);
            SetState(false);
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _UILangMenu.CreateButtons();
        }

        #endregion

        public void SetState(bool state)
        {
            _open = state;
            _animator.SetBool("Open", state);
            if (_open)
                CloseMenuOnCooldown();
        }

        public void CloseMenuOnCooldown()
        {
            if (_closeMenuRoutine != null)
                StopCoroutine(_closeMenuRoutine);
            _closeMenuRoutine = StartCoroutine(CloseMenuRoutine());
        }

        private IEnumerator CloseMenuRoutine()
        {
            yield return new WaitForSeconds(GameManager.instance.menuSettings.timeOutMenu);
            if (_open)
                SetState(false);
        }

        private void Update()
        {
            if (_screenMenu.loaded)
            {
                int nextStyle = 0;
                IHideable previousPage = _screenMenu.GetPreviousPage();
                IHideable nextPage = _screenMenu.GetNextPage();
                IHideable currentPage = _screenMenu.GetCurrentPage();
                _previousButton.SetActive(previousPage != null && currentPage != null && currentPage.HasPrevious());
                _nextButton.SetActive(nextPage != null && currentPage != null && currentPage.HasNext(out nextStyle) && nextStyle == 1);
                _nextButton2.SetActive(nextPage != null && currentPage != null && currentPage.HasNext(out nextStyle) && nextStyle == 2);
                _nextButton3.SetActive(nextPage != null && currentPage != null && currentPage.HasNext(out nextStyle) && nextStyle == 3);
                _text.SetActive(_screenMenu.catchScreen);
                _maskCanvasGroup.interactable = _open;
                _maskCanvasGroup.blocksRaycasts = _open;
            }
        }

        public bool HasNext(out int nextStyle)
        {
            nextStyle = 0;
            return false;
        }

        public bool HasPrevious()
        {
            return false;
        }
    }
}