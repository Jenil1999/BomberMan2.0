using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Level
    [SerializeField]
    List<GameObject> _enemies;

    int i;

    public static Level Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(LevelManager.Instance._currentLevel < 4)
        {
            for(i = 0; i < 8; i++)
            {
                _enemies[i].gameObject.SetActive(true);
            }
        }
        else if(LevelManager.Instance._currentLevel < 7)
        {
            for (i = 4; i < 12; i++)
            {
                _enemies[i].gameObject.SetActive(true);
            }
        }
        else if (LevelManager.Instance._currentLevel <= 10)
        {
            for (i = 6; i < 14; i++)
            {
                _enemies[i].gameObject.SetActive(true);
            }
        }
    }

    public void Destroyer()
    {
        for (i = 0; i < 14; i++)
        {
            _enemies[i].gameObject.SetActive(false);
        }
    }
}
