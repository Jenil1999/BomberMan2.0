using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItems : MonoBehaviour
{
    // PickableItems

    // PowerUpItems
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
        Life,
        ClearAllEnemies,
        WinningGate,
    }

    [Space(10)]
    [Header("PickableObject")]

    [SerializeField] List<Transform> PickableItemPositions;

    int RandomIndexForPickableList;


    private void Start()
    {
        //Random Position
         RandomIndexForPickableList = Random.Range(0, PickableItemPositions.Count);

         transform.position = PickableItemPositions[RandomIndexForPickableList].transform.position; 
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                Destroy(gameObject);
                break;

            case ItemType.BlastRadius:
                player.GetComponent<BombController>()._explosionLength++;
                Destroy(gameObject);
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<PlayerController>()._speed++;
                Destroy(gameObject);
                break;
            case ItemType.Life:
                UiManager.Instance._lifeLine++;
                //Increase Player Life Count ++
                Destroy(gameObject);
                break;
            case ItemType.ClearAllEnemies:
                Level.Instance.Destroyer();
                //Enemy Count = 0 && Destroy All Enemies
                Destroy(gameObject);
                break;
            case ItemType.WinningGate:
                if (UiManager.Instance._enemyCount <= 0)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Sound.GameWin);

                    LevelManager.Instance.NextLevel();
                    PlayerController._isAlive = false;
                }
                break;

        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.PickItem);
            OnItemPickup(other.gameObject);
        }
    }
}
