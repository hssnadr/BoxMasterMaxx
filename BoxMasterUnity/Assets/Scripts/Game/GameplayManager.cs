using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {

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

    private MovementController _mc;

	private TargetController _targetP0;

	private TargetController _targetP1;

	private GameMode _gameMode;

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
        RandomTarget.onHit += OnHit;
        ImpactPointControl.onImpact += OnImpact;
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= OnGameStart;
        GameManager.onGameEnd -= OnGameEnd;
        GameManager.onSetupStart -= OnSetupStart;
        GameManager.onReturnToHome -= OnReturnToHome;
        RandomTarget.onHit -= OnHit;
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
        _playerSetupImage[0].color = GameManager.instance.gameSettings.p1Color.HexToColor();
        _playerSetupImage[0].enabled = false;
        _playerSetupImage[1].color = GameManager.instance.gameSettings.p2Color.HexToColor();
        _playerSetupImage[1].enabled = false;
    }

    private Transform PlayerCanvas(int playerIndex)
    {
        return _playerCanvas[0].transform;
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
        _targetP0 = null;
        _targetP1 = null;
        _mc = null;
    }

	private void OnGameStart(GameMode gameMode, int soloIndex)
    {
		_gameMode = gameMode;
		_soloIndex = soloIndex;
        Transform camera = null;
		if (gameMode == GameMode.P1) {
			var go = GameObject.Instantiate (targetMovementPrefab);
            var controllers = go.GetComponentsInChildren<TargetController>();
            _mc = go;
            _targetP0 = controllers[0];
            _targetP0.playerIndex = GameManager.instance.soloIndex;
            _targetP0.Activate(1);
            camera = GameManager.instance.GetCamera(soloIndex).transform;
            camera.position = new Vector3(
                0.0f,
                Mathf.Clamp(
                    -_playerStartPosition[soloIndex].y,
                    -go.transform.lossyScale.y / 2.0f,
                    go.transform.lossyScale.y / 2.0f
                    ),
                camera.position.z
                );
            _targetP1 = controllers[1];
            _targetP1.enabled = false;
		}
		else
		{
            int rand = Random.Range(0, 2);
			var go = GameObject.Instantiate(targetMovementPrefab);
			var controllers = go.GetComponentsInChildren<TargetController> ();
            _mc = go;
			_targetP0 = controllers [0];
            camera = GameManager.instance.GetCamera(0).transform;
            camera.position = new Vector3(
                0.0f,
                Mathf.Clamp(
                    -_playerStartPosition[0].y,
                    -go.transform.lossyScale.y / 2.0f,
                    go.transform.lossyScale.y / 2.0f
                    ),
                camera.position.z
                );
            _targetP0.playerIndex = 0;
			_targetP0.Activate (rand);
			_targetP1 = controllers [1];
            camera = GameManager.instance.GetCamera(1).transform;
            camera.position = new Vector3(
                0.0f,
                Mathf.Clamp(
                    -_playerStartPosition[1].y,
                    -go.transform.lossyScale.y / 2.0f,
                    go.transform.lossyScale.y / 2.0f
                    ),
                camera.position.z
                );
            _targetP1.playerIndex = 1;
			_targetP1.Activate (1 - rand);

			/*
			go0.playerIndex = 0;
			_targetP0 = go0;
			var go1 = GameObject.Instantiate(targetMovementPrefab);
			go1.playerIndex = 1;
			_targetP1 = go1;
			*/
		}
    }

	private void OnHit(int playerIndex)
    {
		if (_gameMode == GameMode.P1) {
            _targetP0.Activate ();
		} else if (_gameMode == GameMode.P2 && playerIndex == 0)
			_targetP1.Activate ();
        else if (_gameMode == GameMode.P2 && playerIndex == 1)
			_targetP0.Activate ();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnImpact(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), playerIndex);
        }
    }
#endif
}
