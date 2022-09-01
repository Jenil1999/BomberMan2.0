using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{
    // EnemyBehaviour


    public float _speed = 5f;
    bool _isMoving = false;
    bool _isAlive = true;

    float h, v;
    Rigidbody2D _rb2d;

    Animator _animator;

    [SerializeField]
    LayerMask blockingLayer, PlayerLayer , BombLayer;
    //Combined Layer
    int combinedMask;

    enum Direction { Up, Down, Left, Right };
    CircleCollider2D _circleCollider2D;

    bool _followingPlayer;

    //public PlayerController playerControllerScript;
    public int _blockCountsForCheckPlayer;
    public int _blockCountForEmptyPlace;

    private void Start()
    {
        int mask1 = 1 << LayerMask.NameToLayer("Bomb");
        int mask2 = 1 << LayerMask.NameToLayer("Breakable");
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        RandomDirection();
        combinedMask = mask1 | mask2;
       // Debug.Log(mask1 + " " + mask2 + " "  + combinedMask);
    }

    IEnumerator MoveHorizontal(float movementHorizontal, Rigidbody2D rb2d)
    {
        _isMoving = true;

        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        float movementProgress = 0f;
        Vector2 movement, endPos;

        while (movementProgress < Mathf.Abs(movementHorizontal))
        {
            movementProgress += _speed * Time.deltaTime;
            movementProgress = Mathf.Clamp(movementProgress, 0f, 1f);
            movement = new Vector2(_speed * Time.deltaTime * movementHorizontal, 0f);
            endPos = rb2d.position + movement;

            if (movementProgress == 1) endPos = new Vector2(Mathf.Round(endPos.x), endPos.y);
            rb2d.MovePosition(endPos);

            yield return new WaitForFixedUpdate();
        }

        _isMoving = false;
    }

    IEnumerator MoveVertical(float movementVertical, Rigidbody2D rb2d)
    {
        _isMoving = true;

        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        float movementProgress = 0f;
        Vector2 endPos, movement;

        while (movementProgress < Mathf.Abs(movementVertical))
        {

            movementProgress += _speed * Time.deltaTime;
            movementProgress = Mathf.Clamp(movementProgress, 0f, 1f);

            movement = new Vector2(0f, _speed * Time.deltaTime * movementVertical);
            endPos = rb2d.position + movement;

            if (movementProgress == 1) endPos = new Vector2(endPos.x, Mathf.Round(endPos.y));
            rb2d.MovePosition(endPos);
            yield return new WaitForFixedUpdate();

        }
        _isMoving = false;

    }

    private void FixedUpdate()
    {
        if (PlayerController._isAlive)
        {
            StartCoroutine(FollowPlayer());
            if (h != 0 && !_isMoving && _isAlive) StartCoroutine(MoveHorizontal(h, _rb2d));
            else if (v != 0 && !_isMoving && _isAlive) StartCoroutine(MoveVertical(v, _rb2d));
        }
    }



    public void RandomDirection()
    {
        CancelInvoke(nameof(RandomDirection));
        if (!_followingPlayer)
        {

            List<Direction> _randomDirection = new List<Direction>();
            if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, 0), combinedMask))
            {
                _randomDirection.Add(Direction.Right);
            }
            if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(-1, 0), combinedMask))
            {
                _randomDirection.Add(Direction.Left);
            }
            if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, 1), combinedMask))
            {
                _randomDirection.Add(Direction.Up);
            }
            if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, -1), combinedMask))
            {
                _randomDirection.Add(Direction.Down);
            }

            Direction selection = _randomDirection[Random.Range(0, _randomDirection.Count)];
            if (selection == Direction.Up)
            {
                v = 1;
                h = 0;
            }
            if (selection == Direction.Down)
            {
                v = -1;
                h = 0;
            }
            if (selection == Direction.Right)
            {
                v = 0;
                h = 1;
            }
            if (selection == Direction.Left)
            {
                v = 0;
                h = -1;
            }

            Invoke(nameof(RandomDirection), Random.Range(1, 5));
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RandomDirection();
        _followingPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            h = 0;
            v = 0;
            _isAlive = false;
            _circleCollider2D.isTrigger = true;
            _animator.SetTrigger("DemonDead");
            SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDeath);
            Destroy(gameObject, 0.9f);
            UiManager.Instance._enemyCount--;
            UiManager.Instance._gameScore += 100;
            UiManager.Instance.UIDetails();
        }
    }

    IEnumerator FollowPlayer()
    {
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(_blockCountForEmptyPlace, 0), combinedMask))
        {
            //Debug.Log("Right");
            if (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(_blockCountsForCheckPlayer, 0), PlayerLayer))
            {
              //  Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(4, 0), Color.red, 1, false);
                _followingPlayer = true;
                v = 0;
                h = 1;
            }
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(-_blockCountForEmptyPlace, 0), combinedMask))
        {
           
           // Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(-1, 0), Color.white, 2, false);
            //Debug.Log("Left");
            if (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(-_blockCountsForCheckPlayer, 0), PlayerLayer))
            {
               // Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(-4, 0), Color.red, 1, false);
                _followingPlayer = true;
                v = 0;
                h = -1;
            }
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, _blockCountForEmptyPlace), combinedMask))
        {
          //  Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(0, 1), Color.white, 2, false);
            //Debug.Log("Up");
            if (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, _blockCountsForCheckPlayer), PlayerLayer))
            {
             //   Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(0, 4), Color.red, 1, false);
                _followingPlayer = true;
                v = 1;
                h = 0;
            }
        }

        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, -_blockCountForEmptyPlace), combinedMask))
        {
            //Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(0, -1), Color.white, 2, false);
            //Debug.Log("Down");
            if (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, -_blockCountsForCheckPlayer), PlayerLayer))
            {
                //Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(0, -4), Color.red, 1, false);
                _followingPlayer = true;
                v = -1;
                h = 0;
            }
        }
        else
        {
            _followingPlayer = false;
        }

        yield return new WaitForFixedUpdate();
    }

    
}

