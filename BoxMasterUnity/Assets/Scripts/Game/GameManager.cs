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
    public class GameManager : MonoBehaviour
    {
        public delegate void GameManagerEvent(Vector2 position, int playerIndex);
        public static event GameManagerEvent onPlayerSetup;
        /// <summary>
        /// The canvas assigned to the players.
        /// </summary>
        [SerializeField]
        [Tooltip("The canvas assigned to the players.")]
        protected Canvas[] _playerCanvas = new Canvas[ApplicationSettings.PlayerNumber];
        /// <summary>
        /// The setup images assigned to the players.
        /// </summary>
        [SerializeField]
        [Tooltip("The setup images assigned to the players.")]
        protected Image[] _playerSetupImage = new Image[ApplicationSettings.PlayerNumber];
        /// <summary>
        /// The start position assigned to the players.
        /// </summary>
        private Vector2[] _playerStartPosition = new Vector2[ApplicationSettings.PlayerNumber];
        /// <summary>
        /// The cameras of the players.
        /// </summary>
        private Camera[] _playerCamera = new Camera[ApplicationSettings.PlayerNumber];
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
        private TargetController[] _target = new TargetController[ApplicationSettings.PlayerNumber];
        /// <summary>
        /// The current game mode.
        /// </summary>
        private GameMode _gameMode;
        /// <summary>
        /// The game settings.
        /// </summary>
        private ApplicationSettings _gameSettings;
        /// <summary>
        /// The gameplay settings.
        /// </summary>
        private GameSettings _gameplaySettings;
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
        /// The highest combo multiplier.
        /// </summary>
        [SerializeField]
        [Tooltip("The highest combo multiplier.")]
        private int _highestComboMultiplier;

        public int highestComboMultiplier
        {
            get
            {
                return _highestComboMultiplier;
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

        private int _minScore;

        private int _maxScore;
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

        public const string p1BestScoreKey = "P1BestScore";
        public const string p2BestScoreKey = "P2BestScore";

#if UNITY_EDITOR
        public int playerIndex;
#endif

        private void OnEnable()
        {
            ApplicationManager.onGameStart += OnGameStart;
            ApplicationManager.onGameEnd += OnGameEnd;
            ApplicationManager.onSetupStart += OnSetupStart;
            ApplicationManager.onReturnToHome += OnReturnToHome;
            TargetController.onHit += OnHit;
            ImpactPointControl.onImpact += OnImpact;
        }

        private void OnDisable()
        {
            ApplicationManager.onGameStart -= OnGameStart;
            ApplicationManager.onGameEnd -= OnGameEnd;
            ApplicationManager.onSetupStart -= OnSetupStart;
            ApplicationManager.onReturnToHome -= OnReturnToHome;
            TargetController.onHit -= OnHit;
            ImpactPointControl.onImpact -= OnImpact;
        }

        private void OnImpact(Vector2 position, int playerIndex)
        {
            if (ApplicationManager.instance.setupPhase)
            {
                if (_playerSetupImage[playerIndex].enabled)
                {
                    _playerSetupImage[playerIndex].enabled = false;
                    _playerStartPosition[playerIndex] = position;
                    ApplicationManager.instance.LedShutDown(playerIndex);
                    if (onPlayerSetup != null)
                        onPlayerSetup(position, playerIndex);
                }
                if (_gameMode == GameMode.P1 && playerIndex == _soloIndex)
                    ApplicationManager.instance.EndSetup();
                else if (_playerSetupImage.All(x => !x.enabled))
                    ApplicationManager.instance.EndSetup();
            }
        }

        private void Start()
        {
            _gameSettings = ApplicationManager.instance.appSettings;
            _gameplaySettings = ApplicationManager.instance.gameSettings;
            _p1BestScore = PlayerPrefs.HasKey(p1BestScoreKey) ? PlayerPrefs.GetInt(p1BestScoreKey) : 0;
            _p2BestScore = PlayerPrefs.HasKey(p2BestScoreKey) ? PlayerPrefs.GetInt(p2BestScoreKey) : 0;
            _comboMultiplier = _gameplaySettings.comboMin;
            _playerCamera = new Camera[] {
                ApplicationManager.instance.GetCamera(0).GetComponent<Camera>(),
                ApplicationManager.instance.GetCamera(1).GetComponent<Camera>()
            };
            _soloModeActivationDelay = _gameplaySettings.targetP1ActivationDelay;
        }

        private void OnSetupStart(GameMode gameMode, int soloIndex)
        {
            if (gameMode == GameMode.P1)
            {
                ApplicationManager.instance.LedDisplayGrid(soloIndex);
                _playerSetupImage[soloIndex].enabled = true;
            }
            if (gameMode == GameMode.P2)
            {
                ApplicationManager.instance.LedDisplayGrid();
                _playerSetupImage[0].enabled = true;
                _playerSetupImage[1].enabled = true;
            }
        }

        private void OnReturnToHome(HomeOrigin homeOrigin)
        {
            Clean();
        }

        private void OnGameStart(GameMode gameMode, int soloIndex)
        {
            Clean();
            _playerScore = 0;
            _minScore = 0;
            _maxScore = 0;
            _comboMultiplier = _gameplaySettings.comboMin;
            _highestComboMultiplier = _gameplaySettings.comboMin;
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

        private void OnGameEnd()
        {
            Clean();
        }

        private void OnHit(int playerIndex, Vector2 positio, bool successful, Vector3? targetCenter, Vector3? speedVector)
        {
            if (successful)
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
        }

        private void InitModeP1(int soloIndex)
        {
            int otherIndex = soloIndex == 0 ? 1 : 0;
            var go = GameObject.Instantiate(targetMovementPrefab);
            _target = go.GetComponentsInChildren<TargetController>();
            _mc = go;
            InitTarget(_target[soloIndex], _mc, AudioManager.instance, _playerCamera[soloIndex], soloIndex, 1);
            _target[otherIndex].gameObject.SetActive(false);
        }

        private void InitModeP2()
        {
            int rand = Random.Range(0, 2);
            var go = GameObject.Instantiate(targetMovementPrefab);
            _target = go.GetComponentsInChildren<TargetController>();
            _mc = go;
            InitTarget(_target[0], _mc, AudioManager.instance, _playerCamera[0], 0, rand);
            InitTarget(_target[1], _mc, AudioManager.instance, _playerCamera[1], 1, 1 - rand);
        }

        private void InitTarget(TargetController tc, MovementController mc, AudioManager audioManager, Camera camera, int playerIndex, int activation)
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
                audioManager.GetClip(_gameplaySettings.successfulHitAudioPath.key, _gameplaySettings.successfulHitAudioPath.common),
                audioManager.volume,
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
            _minScore = _minScore + _gameplaySettings.hitMinPoints * comboMultiplier;
            _maxScore = _maxScore + _gameplaySettings.hitMaxPoints * comboMultiplier;
            if (playerScore > p1BestScore && _gameMode == GameMode.P1)
            {
                _p1BestScore = playerScore;
                PlayerPrefs.SetInt(p1BestScoreKey, _p1BestScore);
            }
            if (playerScore > p2BestScore && _gameMode == GameMode.P2)
            {
                _p2BestScore = playerScore;
                PlayerPrefs.SetInt(p2BestScoreKey, _p2BestScore);
            }
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
            if ((_maxScore - _minScore) != 0)
                precision *= Mathf.Clamp((((float)_playerScore - _minScore) / ((float)_maxScore - _minScore)), 0.5f, 1.0f);
            precision = Mathf.Sqrt(precision);
            float res = Mathf.Clamp((precision - _gameplaySettings.minPrecisionRating) / (_gameplaySettings.maxPrecisionRating - _gameplaySettings.minPrecisionRating), 0.0f, 1.0f);
            return res;
        }

        private float GetSpeed()
        {
            float activationDelay = _gameMode == GameMode.P1 ? _gameplaySettings.targetP1ActivationDelay : 0.0f;
            float avgHitTime = Mathf.Clamp(_gameplaySettings.gameDuration / Mathf.Max(1.0f, successfulHitCount) - activationDelay, _gameplaySettings.maxSpeedRating, _gameplaySettings.minSpeedRating);
            float res = Mathf.Clamp(1.0f - ((avgHitTime - _gameplaySettings.maxSpeedRating) / (_gameplaySettings.minSpeedRating - _gameplaySettings.maxSpeedRating)), 0.0f, 1.0f);
            return res;
        }
        
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.A))
                playerIndex = 0;
            if (Input.GetKeyUp(KeyCode.Z))
                playerIndex = 1;
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(string.Format("OnImpact {0}", playerIndex));
                OnImpact(ApplicationManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), playerIndex);
            }
#endif
            if (ApplicationManager.instance.gamePhase)
            {
                _comboValue -= 1.0f / (_gameplaySettings.comboDuration * Mathf.Pow(_gameplaySettings.comboDurationMultiplier, comboMultiplier - 1)) * Time.deltaTime;
                if (comboValue > 1.0f && comboMultiplier < _gameplaySettings.comboMax)
                {
                    _comboMultiplier++;
                    _highestComboMultiplier = Mathf.Max(_highestComboMultiplier, comboMultiplier); 
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