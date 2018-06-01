using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class UICatchScreen : UIScreen, IPointerClickHandler
{
    /// <summary>
    /// The Screen Menu.
    /// </summary>
    [SerializeField]
    [Tooltip("The screen menu.")]
    protected UIScreenMenu _UIScreenMenu;

    private string _videoClipPath;

    [SerializeField]
    protected RawImage _videoImage;

    protected override void Start()
    {
        base.Start();
        if (_UIScreenMenu == null)
            _UIScreenMenu = GetComponentInParent<UIScreenMenu>();
        _videoClipPath = GameManager.instance.gameSettings.catchScreenVideoPath;
        VideoManager.instance.AddClip(_videoClipPath);
    }

    public override void Hide()
    {
        base.Hide();
        VideoManager.instance.StopClip();
    }

    public override void Show()
    {
        base.Show();
        VideoManager.instance.PlayClip(_videoClipPath, (RenderTexture)_videoImage.texture);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TextManager.instance.SetDefaultLang();
        _UIScreenMenu.GoToFirstPage();
    }
}
