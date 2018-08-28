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
    /// The prefab of the random target. (only used for testing)
    /// </summary>
    [SerializeField]
    protected RandomTarget randomTargetPrefab;
    /// <summary>
    /// Is the console mode activated ?
    /// </summary>
    [SerializeField]
    protected bool consoleMode;

	private RandomTarget[] _targetP1 = new RandomTarget[2];

	private RandomTarget[] _targetP2 = new RandomTarget[2];

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
        RandomTarget.onHit += OnHit;
        ImpactPointControl.onImpact += OnImpact;
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= OnGameStart;
        GameManager.onGameEnd -= OnGameEnd;
        GameManager.onSetupStart -= OnSetupStart;
        RandomTarget.onHit -= OnHit;
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
		//OnGameStart(GameMode.P2, 0);
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

    private void OnGameEnd()
    {

    }

	private void OnGameStart(GameMode gameMode, int soloIndex)
    {
		_gameMode = gameMode;
		_soloIndex = soloIndex;
		if (gameMode == GameMode.P1) {
			var go = GameObject.Instantiate (randomTargetPrefab, PlayerCanvas (soloIndex));
			go.playerIndex = soloIndex;
		}
		else
		{
			var go00 = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(0));
			go00.playerIndex = 0;
			_targetP1[0] = go00;
			var go01 = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(0));
			go01.playerIndex = 0;
			_targetP1 [1] = go01;
			var go10 = GameObject.Instantiate (randomTargetPrefab, PlayerCanvas (1));
			go10.playerIndex = 1;
			_targetP2 [0] = go10;
			var go11 = GameObject.Instantiate (randomTargetPrefab, PlayerCanvas (1));
			go11.playerIndex = 1;
			_targetP2 [1] = go11;

			bool rand = (Random.Range (0, 2) == 0);
			_targetP1 [0].activated = rand;
			_targetP1 [1].activated = false;
			_targetP2 [0].activated = !rand;
			_targetP2 [1].activated = false;
		}
    }

    private void OnHit(int playerIndex)
    {
        if (_gameMode == GameMode.P1)
        {
            var go = GameObject.Instantiate(randomTargetPrefab, PlayerCanvas(playerIndex));
            go.playerIndex = playerIndex;
        }
		else if (_gameMode == GameMode.P2 && playerIndex == 0)
        {
			int rand = Random.Range (0, 2);
			_targetP1 [0].activated = false;
			_targetP1 [1].activated = false;
			_targetP2 [rand].activated = true;
			_targetP2 [rand == 0 ? 1 : 0].activated = false;
        }
        else if (_gameMode == GameMode.P2 && playerIndex == 1)
        {

			int rand = Random.Range (0, 2);
			_targetP2 [0].activated = false;
			_targetP2 [1].activated = false;
			_targetP1 [rand].activated = true;
			_targetP1 [rand == 0 ? 1 : 0].activated = false;
        }
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
