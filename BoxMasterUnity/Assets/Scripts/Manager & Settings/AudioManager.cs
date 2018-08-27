using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
    [SerializeField]
    private AudioSource _audioSource;

    public float volume
    {
        get
        {
            return _audioSource.volume;
        }
        set
        {
            _audioSource.volume = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        volume = GameManager.instance.gameSettings.audioVolume;

        string[] distinctClipPath = GameManager.instance.gameSettings.pageSettings
            .Where(x => x.GetPageType() == PageSettings.PageType.ContentPage)
            .Select(x => ((ContentPageSettings)x).audioPath)
            .Where(x => x != null)
            .Distinct()
            .ToArray();

        foreach (string clipPath in distinctClipPath)
        {
            AddClip(clipPath);
        }
    }

    public string GetTranslatedClipPath(string clipPath)
    {
        return GetTranslatedClipPath(clipPath, TextManager.instance.currentLang);
    }

    public string GetTranslatedClipPath(string clipPath, LangApp langApp)
    {
        return clipPath + "_" + langApp.code;
    }

    /// <summary>
    /// Adds a clip to the list of clips.
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be loaded.</param>
    public void AddClip(string clipPath)
    {
        foreach (LangApp langApp in GameManager.instance.gameSettings.langAppAvailable)
        {
            try
            {
                var clip = Resources.Load<AudioClip>(GetTranslatedClipPath(clipPath, langApp)) as AudioClip;
                _clips.Add(new AudioClipPath(clip, GetTranslatedClipPath(clipPath, langApp)));
            }
            catch (System.Exception)
            {
                Debug.LogError("File Not Found Exception: " + clipPath);
            }
        }
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
        AudioClipPath clip = _clips.FirstOrDefault(x => x.path == GetTranslatedClipPath(clipPath));

        if (clip == null)
            Debug.LogError("No video for path \"" + clipPath + "\"");
        else if (clip.audioClip != _audioSource.clip)
        {
            _audioSource.Stop();
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
        AudioClipPath clip = _clips.FirstOrDefault(x => x.path == GetTranslatedClipPath(clipPath));
        if (clip == null)
            Debug.LogError("No audio for path \"" + GetTranslatedClipPath(clipPath) + "\"");
        else if (clip.audioClip == _audioSource.clip && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
