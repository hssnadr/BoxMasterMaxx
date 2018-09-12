using CRI.HitBox.Lang;
using CRI.HitBox.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CRI.HitBox.UI
{
    public class UICredits : UIScreen
    {
        /// <summary>
        /// Text of the left side of the content.
        /// </summary>
        [SerializeField]
        [Tooltip("Text of the left side of the content.")]
        private TranslatedText _contentLeftText = null;
        /// <summary>
        /// The of the right side of the content.
        /// </summary>
        [SerializeField]
        [Tooltip("Text of the right side of the content.")]
        private TranslatedText _contentRightText = null;

        protected override IEnumerator Start()
        {
            yield return base.Start();
            CreditsSettings settings = ((CreditsSettings)GameManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == ScreenSettings.ScreenType.Credits));
            StringCommon leftText = settings.leftContent.text;
            StringCommon rightText = settings.rightContent.text;
            _contentLeftText.InitTranslatedText(leftText);
            _contentRightText.InitTranslatedText(rightText);
        }
    }
}
