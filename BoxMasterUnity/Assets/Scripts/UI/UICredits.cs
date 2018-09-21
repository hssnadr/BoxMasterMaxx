using CRI.HitBox.Lang;
using CRI.HitBox.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UICredits : UIScreen
    {
        /// <summary>
        /// Text of the title.
        /// </summary>
        [SerializeField]
        [Tooltip("Text of the title.")]
        private TranslatedText _titleText = null;
        /// <summary>
        /// Text of the margin.
        /// </summary>
        [SerializeField]
        [Tooltip("Text of the margin.")]
        private TranslatedText _marginText = null;
        /// <summary>
        /// Text of the left side of the content.
        /// </summary>
        [SerializeField]
        [Tooltip("Text of the left side of the content.")]
        private TranslatedText _contentLeftText = null;
        /// <summary>
        /// Copyright text of the left side of the content.
        /// </summary>
        [SerializeField]
        [Tooltip("Copyright text of the left side of the content.")]
        private TranslatedText _copyrightLeftText = null;
        /// <summary>
        /// Panel of the logos for the left side of the credits screen.
        /// </summary>
        [SerializeField]
        [Tooltip("Panel of logos for the left side of the credits screen.")]
        private Transform _logosLeftPanel = null;
        /// <summary>
        /// Text of the right side of the content.
        /// </summary>
        [SerializeField]
        [Tooltip("Text of the right side of the content.")]
        private TranslatedText _contentRightText = null;
        /// <summary>
        /// Copyright text of the right side of the content.
        /// </summary>
        [SerializeField]
        [Tooltip("Copyright text of the right side of the content.")]
        private TranslatedText _copyrightRightText = null;
        /// <summary>
        /// Panel of the logos for the right side of the credits screen.
        /// </summary>
        [SerializeField]
        [Tooltip("Panel of logos for the right side of the credits screen.")]
        private Transform _logosRightPanel = null;
        /// <summary>
        /// Prefab of a raw image.
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab of a raw image.")]
        private RawImage _rawImagePrefab = null;
        /// <summary>
        /// The prefered minimal width and height of the texture.
        /// </summary>
        [SerializeField]
        [Tooltip("The prefered minimal width and height of the texture.")]
        private float _preferedTextureMinSize = 0.0f;

        protected override IEnumerator Start()
        {
            yield return base.Start();
            CreditsSettings settings = ((CreditsSettings)GameManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == ScreenSettings.ScreenType.Credits));
            StringCommon title = settings.title;
            StringCommon marginText = settings.marginText;
            StringCommon leftText = settings.leftContent.text;
            StringCommon leftCopyright = settings.leftContent.copyright;
            StringCommon rightText = settings.rightContent.text;
            StringCommon rightCopyright = settings.rightContent.copyright;
            _preferedTextureMinSize = settings.preferedMinTextureSize;
            _titleText.InitTranslatedText(title);
            _marginText.InitTranslatedText(marginText);
            _contentLeftText.InitTranslatedText(leftText);
            _copyrightLeftText.InitTranslatedText(leftCopyright); 
            _contentRightText.InitTranslatedText(rightText);
            _copyrightRightText.InitTranslatedText(rightCopyright);
            InitLogos(_logosLeftPanel, settings.leftContent.logos, TextureManager.instance);
            InitLogos(_logosRightPanel, settings.rightContent.logos, TextureManager.instance);
        }

        private void InitLogos(Transform panel, StringCommon[] logos, TextureManager textureManager)
        {
            if (logos == null)
                return;
            foreach (var logo in logos)
            {
                Texture2D texture = textureManager.GetTexture(logo);
                if (texture != null)
                {
                    var go = GameObject.Instantiate(_rawImagePrefab);
                    go.texture = texture;
                    if (texture.width - _preferedTextureMinSize < texture.height - _preferedTextureMinSize)
                        go.GetComponent<RectTransform>().sizeDelta = new Vector2(_preferedTextureMinSize, texture.height * (_preferedTextureMinSize / texture.width));
                    else
                        go.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width * (_preferedTextureMinSize / texture.height), _preferedTextureMinSize);
                    go.transform.SetParent(panel.transform, false);
                    go.GetComponent<RectTransform>().localScale = Vector3.one;
                    go.GetComponent<RectTransform>().localPosition = new Vector3(go.transform.position.x, go.transform.position.y, 0.0f);
                }
            }
        }
    }
}
