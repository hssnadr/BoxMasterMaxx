using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Game
{
    [RequireComponent(typeof(Animator))]
    public class EndAnimator : MonoBehaviour
    {
        /// <summary>
        /// The gameplay manager.
        /// </summary>
        [SerializeField]
        private GameManager _gameplayManager;

        public void OnEnable()
        {
            ApplicationManager.onGameEnd += OnGameEnd;
        }
        
        public void OnDisable()
        {
            ApplicationManager.onGameEnd -= OnGameEnd;
        }

        private void Start()
        {
            if (_gameplayManager == null)
                _gameplayManager = FindObjectOfType<GameManager>();
        }

        private void OnGameEnd()
        {
            GetComponent<Animator>().SetTrigger("Activate");
        }

        public void OnAnimationEvent()
        {
            _gameplayManager.Clean();
        }
    }
}