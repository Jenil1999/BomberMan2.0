using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
	// BombController

	[Header("Bomb")]
	[SerializeField] GameObject _bombPrefab;

	[SerializeField] int _numberOfBombcanShoot;

	public float _bombFuseTime = 2.5f;

    [Space(10)]
    [Header("Explosion")]
    [SerializeField] Explosion _explosion;
    [SerializeField] float _explosionTime = 0.07f;
    public int _explosionLength = 1;
    public LayerMask _explosionLayerMask;

    [Space(10)]
    [Header("Destroying Bricks")]
    public Grid destructibleTiles;
    public GameObject _breakablePrefab;


    public static BombController insta;

    private void Awake()
    {
        insta = this;
    }

    public void PlantBomb()
	{
		if (_numberOfBombcanShoot > 0)
		{
			StartCoroutine(OnFire());
		}

	}
	IEnumerator OnFire()
	{
        SoundManager.Instance.PlaySound(SoundManager.Sound.BombPlant);
        _numberOfBombcanShoot--;
		Vector2 BombPosition = transform.position;
		BombPosition.x = Mathf.Round(BombPosition.x);
		BombPosition.y = Mathf.Round(BombPosition.y);

		GameObject Bomb = Instantiate(_bombPrefab, BombPosition, transform.rotation);

		yield return new WaitForSeconds(_bombFuseTime);

        //Instantiate Blast Effect...

        Explosion explosion = Instantiate(_explosion, BombPosition, Quaternion.identity);
        //only start explosion

        explosion.SetActiveSprites(explosion.Start);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BombExplosion);
        explosion.DestroyAfter(_explosionTime);

        Explode(BombPosition, Vector2.up, _explosionLength);
        Explode(BombPosition, Vector2.down, _explosionLength);
        Explode(BombPosition, Vector2.left, _explosionLength);
        Explode(BombPosition, Vector2.right, _explosionLength);


        Destroy(Bomb.gameObject);

		_numberOfBombcanShoot++;

	}

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, _explosionLayerMask))
        {
            ClearBreakableBrick(position);
            return;
        }

        Explosion explosion = Instantiate(_explosion, position, Quaternion.identity);

        if (length > 1)      //PowerUp Effect...
        {
            explosion.SetActiveSprites(explosion.Middle);
        }
        else
        {
            explosion.SetActiveSprites(explosion.End);
        }
        explosion.SetDirection(direction);
        explosion.DestroyAfter(_explosionTime);

        Explode(position, direction, length - 1);
    }


    private void ClearBreakableBrick(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.transform.GetChild(0).GetComponent<Tilemap>().GetTile(cell);

        if (tile != null)
        {
           GameObject _blastBrick = Instantiate(_breakablePrefab, position, Quaternion.identity);
            destructibleTiles.transform.GetChild(0).GetComponent<Tilemap>().SetTile(cell, null);

            Destroy(_blastBrick, 1f);
        }
    }

    public void AddBomb()
	{
		_numberOfBombcanShoot++;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
		{
			other.isTrigger = false;
		}
	}

}
