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

    [SerializeField]
    protected RawImage _videoTexture;

    public string videoClipPath = "";

    protected override void Awake()
    {
        base.Awake();
        if (_UIScreenMenu == null)
            _UIScreenMenu = GetComponentInParent<UIScreenMenu>();
        videoClipPath = GameManager.instance.gameSettings.catchScreenVideoPath;
        VideoManager.instance.AddClip(videoClipPath);
    }

    public override void Hide()
    {
        base.Hide();
        VideoManager.instance.StopClip();
    }

    public override void Show()
    {
        base.Show();
        VideoManager.instance.PlayClip(videoClipPath, (RenderTexture)_videoTexture.texture);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _UIScreenMenu.menuBar.SetState(true);
    }
}
