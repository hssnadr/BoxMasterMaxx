using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [Serializable]
    public class AudioClipPath
    {
        public AudioClip audioClip;
        public string path;

        public AudioClipPath(AudioClip audioClip, string path)
        {
            this.audioClip = audioClip;
            this.path = path;
        }
    }

    /// <summary>
    /// Static instance of the audio manager.
    /// </summary>
    public static AudioManager instance
    {
        get
        {
            return s_instance;
        }
    }

    /// <summary>
    /// Static instance of the auio manager.
    /// </summary>
    private static AudioManager s_instance = null;

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

    /// <summary>
    /// List of clips.
    /// </summary>
    private List<AudioClipPath> _clips = new List<AudioClipPath>();
    /// <summary>
    /// The audio source.
    /// </summary>
    [Tooltip("The audio source.")]
    private AudioSource _audioSource;

    private void Start()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        string[] distinctClipPath = GameManager.instance.gameSettings.pageSettings
            .Where(x => x.GetPageType() == PageSettings.PageType.ContentPage)
            .Select(x => ((ContentPageSettings)x).audioPath)
            .Where(x => x != null)
            .Distinct()
            .ToArray();

        foreach (string clipPath in distinctClipPath)
        {
            try
            {
                var clip = Resources.Load<AudioClip>(clipPath) as AudioClip;
                _clips.Add(new AudioClipPath(clip, clipPath));
            }
            catch (System.Exception)
            {
                Debug.LogError("File Not Found Exception: " + clipPath);
            }
        }
    }

    public string GetTranslatedClipPath(string clipPath)
    {
        return clipPath + "_" + TextManager.instance.currentLang.code;
    }

    /// <summary>
    /// Adds a clip to the list of clips.
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be loaded.</param>
    public void AddClip(string clipPath)
    {
        var clip = Resources.Load<AudioClip>(GetTranslatedClipPath(clipPath)) as AudioClip;
        _clips.Add(new AudioClipPath(clip, clipPath));
    }
    /// <summary>
    /// Plays the current clip.
    /// </summary>
    public void PlayClip()
    {
        if (_audioSource.clip != null && !_audioSource.isPlaying)
            _audioSource.Play();
    }

    /// <summary>
    /// Plays the clip corresponding to the parameter's path.
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be played.</param>
    public void PlayClip(string clipPath)
    {
        if (_audioSource.clip != null)
            _audioSource.Stop();

        AudioClipPath clip = _clips.First(x => x.path == GetTranslatedClipPath(clipPath));

        if (clip == null)
            Debug.LogError("No video for path \"" + clipPath + "\"");
        else
        {
            _audioSource.clip = clip.audioClip;
            _audioSource.Play();
        }
    }

    /// <summary>
    /// Stops the clip that is currently playing.
    /// </summary>
    public void StopClip()
    {
        if (_audioSource.clip != null && _audioSource.isPlaying)
            _audioSource.Stop();
    }

    public void StopClip(string clipPath)
    {
        AudioClipPath clip = _clips.First(x => x.path == GetTranslatedClipPath(clipPath));
        if (clip == null)
            Debug.LogError("No audio for path \"" + clipPath + "\"");
        else if (clip.audioClip == _audioSource.clip && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
