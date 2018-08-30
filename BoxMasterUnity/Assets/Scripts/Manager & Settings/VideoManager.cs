using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.IO;

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

    /// <summary>
    /// Static instance of the video manager.
    /// </summary>
    public static VideoManager instance
    {
        get
        {
            return s_instance;
        }
    }

    /// <summary>
    /// Static instance of the video manager.
    /// </summary>
    private static VideoManager s_instance = null;

    /// <summary>
    /// The path of the videos for each language. The [lang_app] value will be replaced by the code of the language. For exemple for French, it will be "fr".
    /// </summary>
    public const string video_lang_base_path = "lang/[lang_app]/video";
    /// <summary>
    /// The path of the videos that are common between all languages.
    /// </summary>
    public const string video_lang_common_path = "lang/Common/video";

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (s_instance != this)
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// List of clips.
    /// </summary>
    private List<VideoClipPath> _clips = new List<VideoClipPath>();
    /// <summary>
    /// The video player.
    /// </summary>
    [SerializeField]
    [Tooltip("The video player.")]
    protected VideoPlayer _videoPlayer;

    private void Start()
    {
        if (_videoPlayer == null)
            _videoPlayer = GetComponent<VideoPlayer>();
        
        PageSettings.StringCommon[] distinctClipPath = GameManager.instance.gameSettings.pageSettings
            .Where(x => x.GetType().GetInterfaces().Contains(typeof(IVideoContainer)))
            .Select(x => ((IVideoContainer)x).GetVideoPath())
            .Where(x => !String.IsNullOrEmpty(x.key))
            .Distinct()
            .ToArray();

        /*
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
        */
    }

    /// <summary>
    /// Adds a clip to the list of clips.
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be loaded.</param>
    public void AddClip(string clipPath)
    {
        var clip = Resources.Load<VideoClip>(clipPath) as VideoClip;
        _clips.Add(new VideoClipPath(clip, clipPath));
    }

    /// <summary>
    /// Gets a clip from the list of clips.
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be given.</param>
    /// <returns>The clip</returns>
    public VideoClip GetClip(string clipPath)
    {
        VideoClipPath clip = _clips.FirstOrDefault(x => x.path == clipPath);
        Debug.Log(clip);
        if (clip == null)
            return null;
        return clip.videoClip;
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
        VideoClipPath clip = _clips.FirstOrDefault (x => x.path == clipPath);

        if (clip == null)
            Debug.LogError("No video for path \"" + clipPath + "\"");
        else
        {
            if (_videoPlayer.clip != null)
                _videoPlayer.Stop();
            _videoPlayer.clip = clip.videoClip;
            _videoPlayer.targetTexture = targetTexture;
            _videoPlayer.Play();
        }
    }

    public string GetVideoPath(string path)
    {
        return Path.Combine(Path.Combine(
            Application.streamingAssetsPath,
            video_lang_base_path.Replace("[lang_app]",
            TextManager.instance.currentLang.code)
            ), path);
    }

    public string GetCommonVideoPath(string path)
    {
        return Path.Combine(Path.Combine(Application.streamingAssetsPath, video_lang_common_path), path);
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
        VideoClipPath clip = _clips.FirstOrDefault(x => x.path == clipPath);
        if (clip == null)
            Debug.LogError("No video for path \"" + clipPath + "\"");
        else if (clip.videoClip == _videoPlayer.clip && _videoPlayer.isPlaying)
            _videoPlayer.Stop();
    }
}
