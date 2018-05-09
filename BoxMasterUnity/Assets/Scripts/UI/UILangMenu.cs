// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILangMenu : MonoBehaviour
{
    [SerializeField]
    protected UILangSelectButton _startLangButtonPrefab;
    [SerializeField]
    protected GameObject _copyrightButtonPrefab;
    [SerializeField]
    protected GameObject _soundButtonPrefab;
    [SerializeField]
    protected GameObject _separatorPrefab;

    private void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        foreach (var buttonType in GameManager.instance.gameSettings.menuLayout)
        {
            switch (buttonType)
            {
                case ButtonType.Start:
                    CreateLangButtons();
                    break;
                case ButtonType.Copyright:
                    CreateCopyrightButton();
                    break;
                case ButtonType.Sound:
                    CreateSoundButton();
                    break;
                case ButtonType.Separator:
                    CreateSeparatorButton();
                    break;
            }
        }
    }

    private void CreateLangButtons()
    {
        foreach (var lang in GameManager.instance.gameSettings.langAppEnable)
        {
            var langSelectButton = GameObject.Instantiate(_startLangButtonPrefab, this.transform);
            langSelectButton.lang = lang;
        }
    }

    private void CreateCopyrightButton()
    {
        GameObject.Instantiate(_copyrightButtonPrefab, this.transform);
    }

    private void CreateSoundButton()
    {
        GameObject.Instantiate(_soundButtonPrefab, this.transform);
    }

    private void CreateSeparatorButton()
    {
        GameObject.Instantiate(_separatorPrefab, this.transform);
    }
}
