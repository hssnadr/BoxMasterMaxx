using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class TextureManager : MonoBehaviour {
    [Serializable]
    public class TexturePath
    {
        public Texture2D texture;
        public string path;
        public string langCode;

        public TexturePath(Texture2D texture, string path, string langCode)
        {
            this.texture = texture;
            this.path = path;
            this.langCode = langCode;
        }
    }

    /// <summary>
    /// Static instance of the audio manager.
    /// </summary>
    public static TextureManager instance
    {
        get
        {
            return s_instance;
        }
    }

    /// <summary>
    /// Static instance of the sprites manager.
    /// </summary>
    private static TextureManager s_instance = null;

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
    /// List of sprites
    /// </summary>
    private List<TexturePath> _textures = new List<TexturePath>();
    /// <summary>
    /// The path of the sprites for each language. The [lang_app] value will be replaced by the code of the language. For exemple for French, it will be "fr".
    /// </summary>
    public const string texture_lang_base_path = "lang/[lang_app]/image";
    /// <summary>
    /// The path of the sprites that are common between all languages.
    /// </summary>
    public const string texture_lang_common_path = "lang/Common/image";

    public bool isDone = false;

    private void Start()
    {
        StringCommon[][] distinctTexturePath = GameManager.instance.gameSettings.screenSettings
            .Where(x => x.GetType().GetInterfaces().Contains(typeof(IImageContainer)))
            .Select(x => ((IImageContainer)x).GetImagePaths())
            .Where(x => x != null)
            .Distinct()
            .ToArray();

        StartCoroutine(LoadTextures(distinctTexturePath));
    }

    public string GetTranslatedTexturePath(string clipPath)
    {
        return Path.Combine(
            Path.Combine(
                Application.streamingAssetsPath,
                texture_lang_base_path.Replace("[lang_app]", TextManager.instance.currentLang.code)
                ),
            clipPath
            );
    }

    public string GetTranslatedTexturePath(string clipPath, LangApp langApp)
    {
        return Path.Combine(
            Path.Combine(
                Application.streamingAssetsPath,
                texture_lang_base_path.Replace("[lang_app]", langApp.code)
                ),
            clipPath
            );
    }

    public string GetCommonTexturePath(string clipPath)
    {
        return Path.Combine(
            Path.Combine(
                Application.streamingAssetsPath,
                texture_lang_common_path),
            clipPath
            );
    }

    IEnumerator LoadTextures(StringCommon[][] distinctTexturePaths)
    {
        isDone = false;
        foreach (var distinctTexturePath in distinctTexturePaths)
        {
            foreach (var stringCommon in distinctTexturePath.Where(x => !String.IsNullOrEmpty(x.key)).Distinct().ToList())
            {
                string texturePath = stringCommon.key;
                bool common = stringCommon.common;
                if (common)
                {
                    string translatedClipPath = GetCommonTexturePath(texturePath);
                    var request = new WWW(translatedClipPath);
                    yield return request;
                    if (!String.IsNullOrEmpty(request.error))
                        Debug.LogError("Error for path \"" + translatedClipPath + "\" : " + request.error);
                    else
                    {
                        var texture = request.texture;
                        texture.name = texturePath;
                        _textures.Add(new TexturePath(texture, texturePath, "Common"));
                    }
                    request.Dispose();
                }
                else
                {
                    foreach (LangApp langApp in GameManager.instance.gameSettings.langAppAvailable)
                    {
                        string translatedClipPath = GetTranslatedTexturePath(texturePath, langApp);
                        var request = new WWW(translatedClipPath);
                        yield return request;
                        if (!String.IsNullOrEmpty(request.error))
                            Debug.LogError("Error for path \"" + translatedClipPath + "\" : " + request.error);
                        else
                        {
                            var texture = request.texture;
                            texture.name = langApp.code + "_" + texturePath;
                            _textures.Add(new TexturePath(texture, texturePath, langApp.code));
                        }
                        request.Dispose();
                    }
                }
            }
        }
        isDone = true;
    }

    public Texture2D GetTexture(string texturePath, bool common)
    {
        Debug.Log(common);
        TexturePath tex = _textures.FirstOrDefault(x => x.path == texturePath && (common || x.langCode == TextManager.instance.currentLang.code));

        if (tex == null)
        {
            Debug.LogError("No texture for path \"" + texturePath + "\"");
            return null;
        }
        return tex.texture;
    }

    public Texture2D GetTexture(StringCommon sc)
    {
        return GetTexture(sc.key, sc.common);
    }

    public bool HasTexture(string texturePath)
    {
        return _textures.Any(x => x.path == texturePath);
    }
}
