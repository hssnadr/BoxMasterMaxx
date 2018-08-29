using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
    [Serializable]
    public class AudioClipPath
    {
        public AudioClip audioClip;
        public string path;
        public string langCode;

        public AudioClipPath(AudioClip audioClip, string path, string langCode)
        {
            this.audioClip = audioClip;
            this.path = path;
            this.langCode = langCode;
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
    /// <summary>
    /// The path of the audios for each language. The [lang_app] value will be replaced by the code of the language. For exemple for French, it will be "fr".
    /// </summary>
    public const string audio_lang_base_path = "lang/[lang_app]/audio";
    /// <summary>
    /// The path of the audio that are common between all languages.
    /// </summary>
    public const string audio_lang_common_path = "lang/Common/audio";

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
        return Path.Combine(
            Path.Combine(
                Application.streamingAssetsPath,
                audio_lang_base_path.Replace("[LangApp]", TextManager.instance.currentLang.code)
                ),
            clipPath
            );
    }

    public string GetTranslatedClipPath(string clipPath, LangApp langApp)
    {
        return Path.Combine(
            Path.Combine(
                Application.streamingAssetsPath,
                audio_lang_base_path.Replace("[LangApp]", langApp.code)
                ),
            clipPath
            );
    }

    public string GetCommonClipPath(string clipPath)
    {
        return Path.Combine(
            Path.Combine(
                Application.streamingAssetsPath,
                audio_lang_common_path),
            clipPath
            );
    }

    /// <summary>
    /// Adds a clip to the list of clips.
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be loaded.</param>
    public void AddClip(string clipPath)
    {
        StartCoroutine(LoadClip(clipPath));
    }

    IEnumerator LoadClip(string clipPath)
    {
        foreach (LangApp langApp in GameManager.instance.gameSettings.langAppAvailable)
        {
            string translatedClipPath = GetTranslatedClipPath(clipPath);
            using (var request = new WWW(GetTranslatedClipPath(translatedClipPath, langApp)))
            {
                yield return request;
                var audioClip = request.GetAudioClip(false);
                audioClip.name = langApp.code + "_" + clipPath;
                _clips.Add(new AudioClipPath(audioClip, clipPath, langApp.code));

                request.Dispose();
            }
        }
    }
    /// <summary>
    /// Plays the current clip.
    /// </summary>
    public void PlayClip()
    {
        if (_audioSource.clip != null && _audioSource.clip.loadState == AudioDataLoadState.Loaded && !_audioSource.isPlaying)
            _audioSource.Play();
    }

    /// <summary>
    /// Plays the clip corresponding to the parameter's path.
    /// </summary>
    /// <param name="clipPath">The path of the clip that will be played.</param>
    public void PlayClip(string clipPath)
    {
        AudioClipPath clip = _clips.FirstOrDefault(x => x.path == clipPath && x.langCode == TextManager.instance.currentLang.code);

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
        AudioClipPath clip = _clips.FirstOrDefault(x => x.path == clipPath && x.langCode == TextManager.instance.currentLang.code);
        if (clip == null)
            Debug.LogError("No audio for path \"" + GetTranslatedClipPath(clipPath) + "\"");
        else if (clip.audioClip == _audioSource.clip && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
