using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIPage<TPageSettings> : MonoBehaviour, IHideable where TPageSettings : PageSettings
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    protected TranslatedText _title;

    protected bool _displayNext;

    protected int _nextStyle = 0;

    public bool displayNext
    {
        get
        {
            return _displayNext;
        }
    }

    public int nextStyle
    {
        get
        {
            return _nextStyle;
        }
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
        nextStyle = this._nextStyle;
        return _displayNext;
    }

    public virtual void Init(TPageSettings pageSettings)
    {
        _title.InitTranslatedText(pageSettings.title);
        _displayNext = pageSettings.displayNext;
        _nextStyle = pageSettings.nextStyle;
    }
}