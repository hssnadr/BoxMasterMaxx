using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIPage : MonoBehaviour, IHideable
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    protected TranslatedText _title;

    public bool displayNext;

    public int nextStyle = 0;

    public TranslatedText title
    {
        get { return _title; }
    }

    protected virtual void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    public virtual void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public virtual void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public bool HasPrevious()
    {
        return false;
    }

    public bool HasNext(out int nextStyle)
    {
        nextStyle = this.nextStyle;
        return displayNext;
    }
}