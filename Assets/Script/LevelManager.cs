using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // LevelManager
    public List<GameObject> _levels;
    public GameObject _map;
    public int _currentLevel;
    int _randomMapNumber;

    public static LevelManager Instance;

    public GameObject _player;

    Vector3 _spawnPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RandomGenerator();
        _spawnPos = _player.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(_map);
            Debug.Log(_map.gameObject.name);
            RandomGenerator();
        }
    }

    public void RandomGenerator()
    {
        _randomMapNumber = Random.Range(0, _levels.Count);

        _map = Instantiate(_levels[_randomMapNumber]);

        BombController.insta.destructibleTiles = _map.GetComponent<Grid>();
    }


    public void RestartLevel()
    {
        UiManager.Instance.OpenGame();

        Destroy(_map);

        RandomGenerator();

        _player.transform.position = _spawnPos;

        /*
             Restart :- keep data As It is
             On video game consoles, the reset button restarts the game, losing the player's unsaved progress.
             & this is Not Reset Function it's Just Restarting Your Current Level And just randomise Map just for make Some Difference, Enemy Difficulties, PowerUp, Your Score and Level Progress will Remain same 
        */
    }

    public void  ResetLevel()
    {
        _currentLevel = 1;
        UiManager.Instance._enemyCount = 8;
        UiManager.Instance._gameScore = 0;
        UiManager.Instance._lifeLine = 3;
        _player.transform.position = _spawnPos;
        Destroy(_map);
        RandomGenerator();
        UiManager.Instance.OpenGame();
        UiManager.Instance.UIDetails();
        //_player.transform.position = _spawnPos;

        //PlayerController._isAlive = true;
        /* 
             Reset : Clear Player's All Progress
             Reset Function will Reset/Clear : Score,powerUps,levels Progress,Time and Your Current Life all Reset or set as Start Game  
        */
    }

    public void GameOver()
    {
        UiManager.Instance.OpenGameOver();
    }

    public void NextLevel()
    {
        _currentLevel++;
        if(_currentLevel>10)
        {
            RestartLevel();
        }
       
        /*
            Level Count ++ till Level.count Reach
            Check Level Script Accordingly Your Level Count If Level Count < 3  : Enemy Sprites Remain Same 
                                                       else If Level Count < 6  : 4 Enemy Sprites Change & Change Them's Behaviour (Enemy Speed + Player Follow radious)
                                                       else If Level Count < 10 : 4 Enemy Sprites From Count < 3 , 2 Will Change From Count < 6 + Add Them's Behaviour Too , 2 new Enemies + Add same Behavior + increase in data 
        */
    }
}
