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
using System.Threading.Tasks;

namespace CRI.HitBox
{
    public class TextureManager : MonoBehaviour
    {
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

        public bool isLoaded = false;

        async void Start()
        {
            isLoaded = false;
            StringCommon[] distinctTexturePath = ApplicationManager.instance.appSettings.allImagePaths;

            await LoadTextures(distinctTexturePath);
            isLoaded = true;
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

        private async Task LoadTextures(StringCommon[] distinctTexturePaths)
        {
            isLoaded = false;
            foreach (var path in distinctTexturePaths)
            {
                string texturePath = path.key;
                bool common = path.common;
                if (common)
                {
                    string translatedClipPath = GetCommonTexturePath(texturePath);
                    var request = await new WWW(translatedClipPath);
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
                    foreach (LangApp langApp in ApplicationManager.instance.appSettings.langAppAvailable)
                    {
                        string translatedClipPath = GetTranslatedTexturePath(texturePath, langApp);
                        var request = await new WWW(translatedClipPath);
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

        public Texture2D GetTexture(string texturePath, bool common)
        {
            TexturePath tex = _textures.FirstOrDefault(x => x.path == texturePath && (common || x.langCode == TextManager.instance.currentLang.code));

            if (tex == null)
            {
                Debug.LogError(string.Format("No texture for {0} path \"{1}\"", common ? "common" : "", texturePath));
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
}