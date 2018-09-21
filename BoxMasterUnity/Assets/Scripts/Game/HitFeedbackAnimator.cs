using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRI.HitBox.Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HitFeedbackAnimator : MonoBehaviour
    {
        /// <summary>
        /// Color of the material
        /// </summary>
        [SerializeField]
        [Tooltip("Color of the material.")]
        private Color _color = Color.white;

        private void Update()
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", _color);
        }

        public void OnAnimationEvent()
        {
            Destroy(this);
            Destroy(gameObject);
        }
    }
}
