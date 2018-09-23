// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CRI.HitBox.Lang;

namespace CRI.HitBox
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
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

        public bool isDone = false;

        private void Start()
        {
            isDone = false;
            _audioSource = GetComponent<AudioSource>();

            volume = ApplicationManager.instance.menuSettings.audioVolume;

            StringCommon[] distinctClipPath = ApplicationManager.instance.appSettings.allAudioPaths;

            StartCoroutine(LoadClips(distinctClipPath));
        }

        public string GetTranslatedClipPath(string clipPath)
        {
            return Path.Combine(
                Path.Combine(
                    Application.streamingAssetsPath,
                    audio_lang_base_path.Replace("[lang_app]", TextManager.instance.currentLang.code)
                    ),
                clipPath
                );
        }

        public string GetTranslatedClipPath(string clipPath, LangApp langApp)
        {
            return Path.Combine(
                Path.Combine(
                    Application.streamingAssetsPath,
                    audio_lang_base_path.Replace("[lang_app]", langApp.code)
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

        private IEnumerator LoadClips(StringCommon[] distinctClipPaths)
        {
            foreach (var paths in distinctClipPaths)
            {
                string clipPath = paths.key;
                Debug.Log(clipPath);
                bool common = paths.common;
                if (common)
                {
                    string translatedClipPath = GetCommonClipPath(clipPath);
                    var request = new WWW(translatedClipPath);
                    yield return request;
                    if (!String.IsNullOrEmpty(request.error))
                        Debug.LogError("Error for path \"" + translatedClipPath + "\" : " + request.error);
                    else
                    {
                        var audioClip = request.GetAudioClip(false, false);
                        audioClip.name = clipPath;
                        _clips.Add(new AudioClipPath(audioClip, clipPath, "Common"));
                    }
                    request.Dispose();
                }
                else
                {
                    foreach (LangApp langApp in ApplicationManager.instance.appSettings.langAppAvailable)
                    {
                        string translatedClipPath = GetTranslatedClipPath(clipPath, langApp);
                        var request = new WWW(translatedClipPath);
                        yield return request;
                        if (!String.IsNullOrEmpty(request.error))
                            Debug.LogError("Error for path \"" + translatedClipPath + "\" : " + request.error);
                        else
                        {
                            var audioClip = request.GetAudioClip(false, false);
                            audioClip.name = langApp.code + "_" + clipPath;
                            _clips.Add(new AudioClipPath(audioClip, clipPath, langApp.code));
                        }
                        request.Dispose();
                    }
                }
            }
            isDone = true;
        }

        public AudioClip GetClip(string clipPath, bool common)
        {
            AudioClipPath clip = _clips.FirstOrDefault(x => x.path == clipPath && (common || x.langCode == TextManager.instance.currentLang.code));
            if (clip == null)
                return null;
            return clip.audioClip;
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
        public void PlayClip(string clipPath, bool common)
        {
            AudioClipPath clip = _clips.FirstOrDefault(x => x.path == clipPath && (common || x.langCode == TextManager.instance.currentLang.code));

            if (clip == null)
                Debug.LogError(string.Format("No audio for {0} path \"{1}\"", common ? "common" : "", common ? GetCommonClipPath(clipPath) : GetTranslatedClipPath(clipPath)));
            else if ((clip.audioClip != _audioSource.clip || !_audioSource.isPlaying) && clip.audioClip.loadState == AudioDataLoadState.Loaded)
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

        public void StopClip(string clipPath, bool common)
        {
            AudioClipPath clip = _clips.FirstOrDefault(x => x.path == clipPath && (common || x.langCode ==  TextManager.instance.currentLang.code));
            if (clip == null)
                Debug.LogError(string.Format("No audio for {0} path \"{1}\"", common ? "common" : "", common ? GetCommonClipPath(clipPath) : GetTranslatedClipPath(clipPath)));
            else if (clip.audioClip == _audioSource.clip && _audioSource.isPlaying)
                _audioSource.Stop();
        }

        public bool HasClip(string clipPath)
        {
            return _clips.Any(x => x.path == clipPath);
        }

        public bool HasClip(string clipPath, string langCode)
        {
            return _clips.Any(x => x.path == clipPath && x.langCode == langCode);
        }
    }
}