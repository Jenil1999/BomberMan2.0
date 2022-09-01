using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class UiManager : MonoBehaviour
{
    // UiManager

    [Space(10)]
    [Header("UI")]
    [SerializeField] Canvas _mainMenu;
    [SerializeField] Canvas _gameOverMenu;
    [Space(10)]
    [Header("MainGameUI")]
    [SerializeField] Canvas _mainGame;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _life;
    [SerializeField] TextMeshProUGUI _timeDisplay;
    [SerializeField] TextMeshProUGUI _enemyCountDisplay;
    [Space(5)]
    [SerializeField] float _playingTime;
    float _minusTime;
    [Space(10)]
    [Header("LoadingUI")]
    [SerializeField] Canvas _loadingLevel;
    [SerializeField] TextMeshProUGUI _titleNumberText;
    [Header("GamePauseUI")]
    [SerializeField] Canvas _gamePauseMenu;

    public float _lifeLine;

    public float _enemyCount;

    public int _gameScore;

    public static UiManager Instance;

    private void Start()
    {
        _lifeLine = 3;
        _enemyCount = 8;
        _gameScore = 0;
        _minusTime = _playingTime;
        UIDetails();
        MainMenu();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _minusTime -= Time.deltaTime;
        _timeDisplay.text = Mathf.Round(_minusTime).ToString();
    }

    public void UIDetails()
    {
        _score.text = _gameScore.ToString();
        _life.text = _lifeLine.ToString();
        _enemyCountDisplay.text = _enemyCount.ToString();
    }

    public void MainMenu()
     {
        SoundManager.Instance.PlayBGM(SoundManager.BGMSound.MainMenu);
        Time.timeScale = 0;
        _mainMenu.enabled = true;
        _mainGame.enabled = false;
        _gameOverMenu.enabled = false;
        _loadingLevel.enabled = false;
    }

    public void Loading()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGMSound.Loading);

        _titleNumberText.text = (LevelManager.Instance._currentLevel).ToString();

        _mainMenu.enabled = false;
        _mainGame.enabled = false;
        _gameOverMenu.enabled = false;
        _loadingLevel.enabled = true;

    }

    async public void OpenGame()
    {
        Loading();

        await Task.Delay(2500);

        Time.timeScale = 1;

        _minusTime = _playingTime;

        PlayerController._isAlive = true;
        SoundManager.Instance.PlayBGM(SoundManager.BGMSound.MainGame);
      
        _loadingLevel.enabled = false;
        _mainMenu.enabled = false;
        _mainGame.enabled = true;
        _gameOverMenu.enabled = false;

    }

    public void OpenGameOver()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGMSound.GameOver);
        SoundManager.Instance.BGMAudioSource.loop = true;
        _mainMenu.enabled = false;
        _mainGame.enabled = false;
        _gameOverMenu.enabled = true;
    }

    public void OnGamePause()
    {
        Time.timeScale = 0;

        _gamePauseMenu.enabled = true;
    }

    public void OnGamePlay()
    {
        Time.timeScale = 1;
        _mainGame.enabled = true;
        _gamePauseMenu.enabled = false;
    }

}
