// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Serial;
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

        /// <summary>
        /// How each hit is multiplied for the final score.
        /// </summary>
        [SerializeField]
        [Tooltip("How each hit is multiplied for the final score.")]
        private int _comboMultiplier;
        /// <summary>
        /// How each hit is multiplied for the final score.
        /// </summary>
        public int comboMultiplier
        {
            get
            {
                return _comboMultiplier;
            }
        }

        /// <summary>
        /// Number of successful hits.
        /// </summary>
        [SerializeField]
        [Tooltip("Number of successful hits.")]
        private int _successfulHitCount;
        /// <summary>
        /// Number of successful hits.
        /// </summary>
        public int successfulHitCount
        {
            get
            {
                return _successfulHitCount;
            }
        }

        /// <summary>
        /// The number of hits.
        /// </summary>
        [SerializeField]
        [Tooltip("The number of hits.")]
        private int _hitCount;
        /// <summary>
        /// The number of hits.
        /// </summary>
        public int hitCount
        {
            get
            {
                return _hitCount;
            }
        }

        /// <summary>
        /// The combo count. When it hits the threshold value described in the game settings, the combo multiplier increases.
        /// </summary>
        [SerializeField]
        [Tooltip("The combo count. When it hits the threshold value described in the game settings, the combo multiplier increases.")]
        private float _comboValue;
        /// <summary>
        /// The combo count. When it hits the threshold value described in the game settings, the combo multiplier increases.
        /// </summary>
        public float comboValue
        {
            get
            {
                return _comboValue;
            }
        }

        /// <summary>
        /// Score of the players.
        /// </summary>
        [SerializeField]
        [Tooltip("Score of the players.")]
        private int _playerScore;
        /// <summary>
        /// Score of the players.
        /// </summary>
        public int playerScore
        {
            get
            {
                return _playerScore;
            }
        }

        /// <summary>
        /// Best score of the P1 mode.
        /// </summary>
        [SerializeField]
        [Tooltip("Best score the the P1 mode.")]
        private int _p1BestScore;
        /// <summary>
        /// Best score of the P1 mode.
        /// </summary>
        public int p1BestScore
        {
            get
            {
                return _p1BestScore;
            }
        }

        /// <summary>
        /// Best score of the P2 mode.
        /// </summary>
        [SerializeField]
        [Tooltip("Best score of the P2 mode.")]
        private int _p2BestScore;
        /// <summary>
        /// Best score of the P2 mode
        /// </summary>
        public int p2BestScore
        {
            get
            {
                return _p2BestScore;
            }
        }

        /// <summary>
        /// Value between 0 and 1 that tells the precision of the players.
        /// </summary>
        public float precision
        {
            get
            {
                return GetPrecision();
            }
        }

        /// <summary>
        /// Value between 0 and 1 that tells the speed of the players.
        /// </summary>
        public float speed
        {
            get
            {
                return GetSpeed();
            }
        }

#if UNITY_EDITOR
        public int playerIndex;
#endif

        private void OnEnable()
        {
            GameManager.onGameStart += OnGameStart;
            GameManager.onSetupStart += OnSetupStart;
            GameManager.onReturnToHome += OnReturnToHome;
            TargetController.onSuccessfulHit += OnSuccessfulHit;
            ImpactPointControl.onImpact += OnImpact;
        }

        private void OnDisable()
        {
            GameManager.onGameStart -= OnGameStart;
            GameManager.onSetupStart -= OnSetupStart;
            GameManager.onReturnToHome -= OnReturnToHome;
            TargetController.onSuccessfulHit -= OnSuccessfulHit;
            ImpactPointControl.onImpact -= OnImpact;
        }

        private void OnImpact(Vector2 position, int playerIndex)
        {
            if (GameManager.instance.setupPhase)
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
            var database = GameManager.instance.database;
            _p1BestScore = database.GetNumberOfPlayers(GameMode.P1) > 0 ? database.GetBestScore(GameMode.P1) : 0;
            _p2BestScore = database.GetNumberOfPlayers(GameMode.P2) > 0 ? database.GetBestScore(GameMode.P2) : 0;
            _comboMultiplier = _gameplaySettings.comboMin;
            _playerCamera = new Camera[] {
                GameManager.instance.GetCamera(0).GetComponent<Camera>(),
                GameManager.instance.GetCamera(1).GetComponent<Camera>()
            };
            Color p1Color = Color.white;
            Color p2Color = Color.white;
            if (ColorUtility.TryParseHtmlString(_gameSettings.p1Color, out p1Color))
            {
                _playerSetupImage[0].color = p1Color;
            }
            _playerSetupImage[0].enabled = false;
            if (ColorUtility.TryParseHtmlString(_gameSettings.p2Color, out p2Color))
            {
                _playerSetupImage[1].color = p2Color;
            }
            _playerSetupImage[1].enabled = false;
            _soloModeActivationDelay = _gameplaySettings.targetP1ActivationDelay;
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
            Clean();
        }

        private void OnGameStart(GameMode gameMode, int soloIndex)
        {
            Clean();
            _playerScore = 0;
            _comboMultiplier = _gameplaySettings.comboMin;
            _comboValue = 0;
            _successfulHitCount = 0;
            _hitCount = 0;
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
            InitTarget(_target[1], _mc, _playerCamera[1], 1, 1 - rand);
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
                this,
                _gameplaySettings.hitMinPoints,
                _gameplaySettings.hitMaxPoints,
                _gameplaySettings.hitTolerance,
                _gameplaySettings.targetCooldown,
                activation);
        }

        public void Clean()
        {
            _playerSetupImage[0].enabled = false;
            _playerSetupImage[1].enabled = false;
            _target[0] = null;
            _target[1] = null;
            if (_mc != null)
            {
                Destroy(_mc.gameObject);
                _mc = null;
            }
        }

        /// <summary>
        /// Called whenever the player misses.
        /// </summary>
        public void Miss()
        {
            _hitCount++;
        }

        /// <summary>
        /// Increases the score of the players.
        /// </summary>
        public void ScoreUp(int score = 1)
        {
            _successfulHitCount++;
            _hitCount++;
            _comboValue += _gameplaySettings.comboIncrement;
            _playerScore = playerScore + score * comboMultiplier;
            if (playerScore > p1BestScore && _gameMode == GameMode.P1)
                _p1BestScore = playerScore;
            if (playerScore > p2BestScore && _gameMode == GameMode.P2)
                _p2BestScore = playerScore;
        }

        public int GetBestScore(GameMode mode)
        {
            if (mode == GameMode.P1)
                return p1BestScore;
            else
                return p2BestScore;
        }

        private IEnumerator ActivateWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (_target[_soloIndex] != null)
                _target[_soloIndex].Activate(1);
        }

        private float GetPrecision()
        {
            float precision = (float)successfulHitCount / Mathf.Max(1.0f, hitCount);
            float res = Mathf.Clamp((precision - _gameplaySettings.minPrecisionRating) / (_gameplaySettings.maxPrecisionRating - _gameplaySettings.minPrecisionRating), 0.0f, 1.0f);
            Debug.Log(string.Format("({0} - {1}) / ({2} - {1}) = {3}", precision, _gameplaySettings.minPrecisionRating, _gameplaySettings.maxPrecisionRating, res));
            return res;
        }

        private float GetSpeed()
        {
            float activationDelay = _gameMode == GameMode.P1 ? _gameplaySettings.targetP1ActivationDelay : 0.0f;
            float avgHitTime = Mathf.Clamp(_gameplaySettings.gameDuration / Mathf.Max(1.0f, successfulHitCount) - activationDelay, _gameplaySettings.maxSpeedRating, _gameplaySettings.minSpeedRating);
            float res = Mathf.Clamp(1.0f - ((avgHitTime - _gameplaySettings.maxSpeedRating) / (_gameplaySettings.minSpeedRating - _gameplaySettings.maxSpeedRating)), 0.0f, 1.0f);
            Debug.Log(string.Format("{0} - ({1} - {2} / {3} - {2}) = {4}", 1.0f, avgHitTime, _gameplaySettings.maxSpeedRating, _gameplaySettings.minSpeedRating, res));
            return res;
        }
        
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.A))
                playerIndex = 0;
            if (Input.GetKeyUp(KeyCode.Z))
                playerIndex = 1;
            if (Input.GetMouseButton(0))
            {
                OnImpact(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), playerIndex);
            }
#endif
            if (GameManager.instance.gamePhase)
            {
                _comboValue -= 1.0f / (_gameplaySettings.comboDuration * Mathf.Pow(_gameplaySettings.comboDurationMultiplier, comboMultiplier - 1)) * Time.deltaTime;
                if (comboValue > 1.0f && comboMultiplier < _gameplaySettings.comboMax)
                {
                    _comboMultiplier++;
                    _comboValue -= 1.0f;
                }
                if (comboValue < 0.0f && comboMultiplier > _gameplaySettings.comboMin)
                {
                    _comboMultiplier--;
                    _comboValue += 1.0f;
                }
                _comboValue = Mathf.Clamp(comboValue, 0.0f, 1.0f);
            }
        }
    }
}