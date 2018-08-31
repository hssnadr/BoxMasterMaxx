using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImagePage : UIPage<PageSettings> {
    [SerializeField]
    protected RawImage _rawImage;
    [SerializeField]
    protected RawImage _videoTexture;

    public string videoClipPath = "";

    public RawImage rawImage
    {
        get { return _rawImage; }
    }

    public RawImage videoTexture
    {
        get { return _videoTexture; }
    }
    public override void Hide()
    {
        base.Hide();
        VideoManager.instance.StopClip();
    }

    public override void Show()
    {
        base.Show();
        if (videoClipPath != "" && _videoTexture.enabled)
            VideoManager.instance.PlayClip(videoClipPath, (RenderTexture)_videoTexture.texture);
    }
}
