// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UILoadingScreen : UIScreen
    {
        [SerializeField]
        private Text _loadingText = null;

        private UIScreenMenu _screenMenu = null;

        private Coroutine _coroutine = null;

        protected override IEnumerator Start()
        {
            if (_screenMenu == null)
                _screenMenu = GameObject.FindObjectOfType<UIScreenMenu>();
            yield return base.Start();
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
            if (!_screenMenu.loaded && !_visible)
                Show();
            if (_screenMenu.loaded && _visible)
                Hide();
        }

        public override void Hide()
        {
            base.Hide();
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
    }
}
