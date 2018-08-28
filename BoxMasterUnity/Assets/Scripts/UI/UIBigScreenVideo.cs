using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIBigScreenVideo : UIScreen {
    /// <summary>
    /// The video player.
    /// </summary>
    [SerializeField]
    [Tooltip("The video player.")]
    private VideoPlayer _videoPlayer = null;

    protected void Start()
    {
        string videoClipPath = GameManager.instance.gameSettings.bigScreenVideoPath;
        VideoManager.instance.AddClip(videoClipPath);
        _videoPlayer.clip = VideoManager.instance.GetClip(videoClipPath);
        _videoPlayer.targetTexture = (RenderTexture)GetComponent<RawImage>().texture;
        Show();
    }

    public override void Show()
    {
        base.Show();
        _videoPlayer.Play();
    }

    public override void Hide()
    {
        base.Hide();
        _videoPlayer.Stop();
    }
}
