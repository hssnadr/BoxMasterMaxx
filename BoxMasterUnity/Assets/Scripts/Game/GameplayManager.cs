// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Arduino;
using CRI.HitBox.Settings;

namespace CRI.HitBox.Game
{
    /// <summary>
    /// This class manages the boxing experience of the game.
    /// </summary>
    public class GameplayManager : MonoBehaviour
    {
        /// <summary>
        /// The canvas assigned to the players.
        /// </summary>
        [SerializeField]
        [Tooltip("The canvas assigned to the players.")]
        protected Canvas[] _playerCanvas = new Canvas[GameSettings.PlayerNumber];
        /// <summary>
        /// The setup images assigned to the players.
        /// </summary>
        [SerializeField]
        [Tooltip("The setup images assigned to the players.")]
        protected Image[] _playerSetupImage = new Image[GameSettings.PlayerNumber];
        /// <summary>
        /// The start position assigned to the players.
        /// </summary>
        private Vector2[] _playerStartPosition = new Vector2[GameSettings.PlayerNumber];
        /// <summary>
        /// The cameras of the players.
        /// </summary>
        private Camera[] _playerCamera = new Camera[GameSettings.PlayerNumber];
        /// <summary>
        /// The prefab of the random target.
        /// </summary>
        [SerializeField]
        [Tooltip("The prefab of the random target.")]
        protected MovementController targetMovementPrefab;
        /// <summary>
        /// Is the console mode activated ?
        /// </summary>
        [SerializeField]
        protected bool consoleMode;
        /// <summary>
        /// The delay before target activation in solo mode.
        /// </summary>
        [SerializeField]
        [Tooltip("The delay before target activation in solo mode.")]
        private float _soloModeActivationDelay;
        /// <summary>
        /// The movement controller.
        /// </summary>
        private MovementController _mc;
        /// <summary>
        /// The target controllers. One for each player.
        /// </summary>
        private TargetController[] _target = new TargetController[GameSettings.PlayerNumber];
        /// <summary>
        /// The current game mode.
        /// </summary>
        private GameMode _gameMode;
        /// <summary>
        /// The game settings.
        /// </summary>
        private GameSettings _gameSettings;
        /// <summary>
        /// The gameplay settings.
        /// </summary>
        private GameplaySettings _gameplaySettings;
        /// <summary>
        /// The current index of the player in solo mode.
        /// </summary>
        private int _soloIndex;

#if UNITY_EDITOR
        public int playerIndex;
#endif

        private void OnEnable()
        {
            GameManager.onGameStart += OnGameStart;
            GameManager.onGameEnd += OnGameEnd;
            GameManager.onSetupStart += OnSetupStart;
            GameManager.onReturnToHome += OnReturnToHome;
            TargetController.onSuccessfulHit += OnSuccessfulHit;
            ImpactPointControl.onImpact += OnImpact;
        }

        private void OnDisable()
        {
            GameManager.onGameStart -= OnGameStart;
            GameManager.onGameEnd -= OnGameEnd;
            GameManager.onSetupStart -= OnSetupStart;
            GameManager.onReturnToHome -= OnReturnToHome;
            TargetController.onSuccessfulHit -= OnSuccessfulHit;
            ImpactPointControl.onImpact -= OnImpact;
        }

        private void OnImpact(Vector2 position, int playerIndex)
        {
            if (GameManager.instance.gameState == GameState.Setup)
            {
                if (_playerSetupImage[playerIndex].enabled)
                {
                    _playerSetupImage[playerIndex].enabled = false;
                    _playerStartPosition[playerIndex] = position;
                }
                if (_playerSetupImage.All(x => !x.enabled))
                    GameManager.instance.EndSetup();
            }
        }

        private void Start()
        {
            _gameSettings = GameManager.instance.gameSettings;
            _gameplaySettings = GameManager.instance.gameplaySettings;
            _playerCamera = new Camera[] {
                GameManager.instance.GetCamera(0).GetComponent<Camera>(),
                GameManager.instance.GetCamera(1).GetComponent<Camera>()
            };
            Color p1Color = Color.white;
            Color p2Color = Color.white;
            if (ColorUtility.TryParseHtmlString(_gameSettings.p1Color, out p1Color))
                _playerSetupImage[0].color = p1Color;
            _playerSetupImage[0].enabled = false;
            if (ColorUtility.TryParseHtmlString(_gameSettings.p2Color, out p2Color))
                _playerSetupImage[1].color = p2Color;
            _playerSetupImage[1].enabled = false;
            _soloModeActivationDelay = _gameplaySettings.targetActivationDelay;
        }

        private void OnSetupStart(GameMode gameMode, int soloIndex)
        {
            if (gameMode == GameMode.P1)
            {
                _playerSetupImage[soloIndex].enabled = true;
            }
            if (gameMode == GameMode.P2)
            {
                _playerSetupImage[0].enabled = true;
                _playerSetupImage[1].enabled = true;
            }
        }

        private void OnReturnToHome()
        {
            OnGameEnd();
        }

        private void OnGameEnd()
        {
            _playerSetupImage[0].enabled = false;
            _playerSetupImage[1].enabled = false;
            _target[0] = null;
            _target[1] = null;
            _mc = null;
        }

        private void OnGameStart(GameMode gameMode, int soloIndex)
        {
            _playerSetupImage[0].enabled = false;
            _playerSetupImage[1].enabled = false;
            _gameMode = gameMode;
            _soloIndex = soloIndex;
            if (gameMode == GameMode.P1)
                InitModeP1(soloIndex);
            else
                InitModeP2();
        }

        private void OnSuccessfulHit(int playerIndex)
        {
            if (_gameMode == GameMode.P1)
            {
                StartCoroutine(ActivateWithDelay(_soloModeActivationDelay));
            }
            else if (_gameMode == GameMode.P2 && playerIndex == 0)
                _target[1].Activate();
            else if (_gameMode == GameMode.P2 && playerIndex == 1)
                _target[0].Activate();
        }

        private void InitModeP1(int soloIndex)
        {
            int otherIndex = soloIndex == 0 ? 1 : 0;
            var go = GameObject.Instantiate(targetMovementPrefab);
            _target = go.GetComponentsInChildren<TargetController>();
            _mc = go;
            InitTarget(_target[soloIndex], _mc, _playerCamera[soloIndex], soloIndex, 1);
            _target[otherIndex].gameObject.SetActive(false);
        }

        private void InitModeP2()
        {
            int rand = Random.Range(0, 2);
            var go = GameObject.Instantiate(targetMovementPrefab);
            _target = go.GetComponentsInChildren<TargetController>();
            _mc = go;
            InitTarget(_target[0], _mc, _playerCamera[0], 0, rand);
            InitTarget(_target[1], _mc, _playerCamera[1], 1, rand);
        }

        private void InitTarget(TargetController tc, MovementController mc, Camera camera, int playerIndex, int activation)
        {
            camera.transform.position = new Vector3(
                0.0f,
                Mathf.Clamp(
                    -_playerStartPosition[1].y,
                    -mc.transform.lossyScale.y / 2.0f,
                    mc.transform.lossyScale.y / 2.0f
                    ),
                camera.transform.position.z
                );
            tc.Init(playerIndex,
                camera,
                _gameplaySettings.minPoints,
                _gameplaySettings.maxPoints,
                _gameplaySettings.tolerance,
                _gameplaySettings.targetCooldown,
                activation);
        }

        private IEnumerator ActivateWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (_target[_soloIndex] != null)
                _target[_soloIndex].Activate(1);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
                playerIndex = 0;
            if (Input.GetKeyUp(KeyCode.Z))
                playerIndex = 1;
            if (Input.GetMouseButton(0))
            {
                OnImpact(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), playerIndex);
            }
        }
#endif
    }
}