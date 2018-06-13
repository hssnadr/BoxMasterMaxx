using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class VideoManager : MonoBehaviour {
    [Serializable]
    public class VideoClipPath
    {
        public VideoClip videoClip;
        public string path;

        public VideoClipPath(VideoClip videoClip, string path)
        {
            this.videoClip = videoClip;
            this.path = path;
        }
    }

    public static VideoManager instance
    {
        get
        {
            return s_instance;
        }
    }

    private static VideoManager s_instance = null;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (s_instance != this)
        {
            Destroy(this);
        }
    }

    public List<VideoClipPath> _clips = new List<VideoClipPath>();
    [SerializeField]
    protected VideoPlayer _videoPlayer;

    private void Start()
    {
        if (_videoPlayer == null)
            _videoPlayer = GetComponent<VideoPlayer>();

        string[] distinctClipPath = GameManager.instance.gameSettings.pageSettings
            .Where(x => x.GetPageType() == PageSettings.PageType.ContentPage)
            .Select(x => ((ContentPageSettings)x).videoPath)
            .Where(x => x != null)
            .Distinct()
            .ToArray();

        foreach (string clipPath in distinctClipPath)
        {
            try
            {
                var clip = Resources.Load<VideoClip>(clipPath) as VideoClip;
                _clips.Add(new VideoClipPath(clip, clipPath));
            }
            catch (System.Exception)
            {
                Debug.LogError("File Not Found Exception: " + clipPath);
            }
        }
    }

    public void AddClip(string clipPath)
    {
        var clip = Resources.Load<VideoClip>(clipPath) as VideoClip;
        _clips.Add(new VideoClipPath(clip, clipPath));
    }

    /// <summary>
    /// Plays the current clip.
    /// </summary>
    public void PlayClip()
    {
        if (_videoPlayer.clip != null && !_videoPlayer.isPlaying)
            _videoPlayer.Play();
    }

    /// <summary>
    /// Plays the clip corresponding to the parameter's path
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be played</param>
    public void PlayClip(string clipPath, RenderTexture targetTexture)
    {
        if (_videoPlayer.clip != null)
            _videoPlayer.Stop();

        VideoClipPath clip = _clips.First(x => x.path == clipPath);

        if (clip == null)
            Debug.LogError("No video for path \"" + clipPath + "\"");
        else
        {
            _videoPlayer.clip = clip.videoClip;
            _videoPlayer.targetTexture = targetTexture;
            _videoPlayer.Play();
        }
    }

    /// <summary>
    /// Stops the clip that is currently playing
    /// </summary>
    public void StopClip()
    {
        if (_videoPlayer.clip != null && _videoPlayer.isPlaying)
            _videoPlayer.Stop();
    }

    /// <summary>
    /// Stops a specific clip.
    /// </summary>
    /// <param name="clipPath">Path of the clip that will be stopped</param>
    public void StopClip(string clipPath)
    {
        VideoClipPath clip = _clips.First(x => x.path == clipPath);
        if (clip == null)
            Debug.LogError("No video for path \"" + clipPath + "\"");
        else if (clip.videoClip == _videoPlayer.clip && _videoPlayer.isPlaying)
            _videoPlayer.Stop();
    }
}
