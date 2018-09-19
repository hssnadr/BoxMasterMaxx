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
        private GameplayManager _gameplayManager;

        public void OnEnable()
        {
            GameManager.onGameEnd += OnGameEnd;
        }
        
        public void OnDisable()
        {
            GameManager.onGameEnd -= OnGameEnd;
        }

        private void Start()
        {
            if (_gameplayManager == null)
                _gameplayManager = FindObjectOfType<GameplayManager>();
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