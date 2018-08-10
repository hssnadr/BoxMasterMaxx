using System;
using System.Collections.Generic;
using UnityEngine;

public class UISurveyQuestion : MonoBehaviour
{
    public delegate void SurveyEvent(
        string questionKey,
        string answerKey,
        SurveyAnswer.ButtonAction action,
        int index,
        int playerIndex
        );
    public static event SurveyEvent onAnswer;
    [SerializeField]
    protected TranslatedText _text;
    [SerializeField]
    protected Transform _buttonPanel;
    [SerializeField]
    protected UISurveyButton _buttonPrefab;
    [SerializeField]
    protected List<UISurveyButton> _buttonList;
    [SerializeField]
    protected float _disabledAlphaValue = 0.3f;

    private int _index;
    private int _playerIndex;
    private string _questionKey;
    private bool _interactable;

    public bool interactable
    {
        get {
            return _interactable;
        }
        set
        {
            SetInteractable(value);
        }
    }


    public void Init(SurveyQuestion question, int index, int playerIndex, Color color1, Color color2)
    {
        _index = index;
        _questionKey = question.key;
        _playerIndex = playerIndex;
        _text.InitTranslatedText(_questionKey);
        int i = 0;
        foreach (SurveyAnswer answer in question.answers)
        {
            var button = GameObject.Instantiate(_buttonPrefab, _buttonPanel);
            button.Init(answer, i % 2 == 0 ? color1 : color2);
            button.onAnswer += OnAnswer;
            _buttonList.Add(button);
            i++;
        }
    }

    public void Reset()
    {
        foreach (var button in _buttonList)
        {
            button.Highlight(false);
        }
    }

    private void SetInteractable(bool interactable)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = interactable ? 1.0f : _disabledAlphaValue;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
        _interactable = interactable;

        if (!interactable)
        {
            foreach (var button in _buttonList)
                button.Highlight(false);
        }
    }

    private void OnAnswer(string answerKey, SurveyAnswer.ButtonAction action, UISurveyButton answerButton)
    {
        answerButton.Highlight(true);
        foreach (var button in _buttonList)
        {
            if (button != answerButton)
                button.Highlight(false);
        }
        onAnswer(_questionKey, answerKey, action, _index, _playerIndex);
    }

    private void OnEnable()
    {
        foreach (var button in _buttonList)
        {
            button.onAnswer += OnAnswer;
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttonList)
        {
            button.onAnswer -= OnAnswer;
        }
    }
}