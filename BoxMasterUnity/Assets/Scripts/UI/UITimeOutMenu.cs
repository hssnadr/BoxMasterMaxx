using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeOutMenu : UIScreen {
    private string _videoClipPath;
    [SerializeField]
    protected RawImage _videoTexture;

    protected override void Awake()
    {
        base.Awake();
        _videoClipPath = GameManager.instance.menuSettings.catchScreenVideoPath;
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
        VideoManager.instance.PlayClip(_videoClipPath, (RenderTexture)_videoTexture.texture);
    }
}
