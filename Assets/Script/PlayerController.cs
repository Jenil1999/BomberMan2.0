using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    // PlayerController
    // PlayerMovement

    public int _speed = 5;
    bool _isMoving = false;
    float h, v;
    Rigidbody2D _rb2d;
    SpriteRenderer _spriteRenderer;
    // public bool _isAlive = true;
    // Animation

    BoxCollider2D _boxCollider2D;
    public static bool _isAlive = true;

    Animator _animator;

    //public delegate void Mydelegate();
    //public static Mydelegate OnMydelegate;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (h != 0 && !_isMoving) StartCoroutine(MoveHorizontal(h, _rb2d));
        else if (v != 0 && !_isMoving) StartCoroutine(MoveVertical(v, _rb2d));
    }

    public void MovingLeft()
    {
        if (_isAlive)
        {
            h = -1;
            _animator.SetBool("IsRunning", true);
            _spriteRenderer.flipX = false;
        }
    }

    public void MovingRight()
    {
        if (_isAlive)
        {
            h = 1;
            _animator.SetBool("IsRunning", true);
            _spriteRenderer.flipX = true;
        }
    }

    public void MovingUp()
    {
        if (_isAlive)
        {
            v = 1;
            _animator.SetBool("RunningUp", true);
        }
    }

    public void MovingDown()
    {
        if (_isAlive)
        {
            v = -1;
            _animator.SetBool("RunningDown", true);
        }
    }

    public void stopMoving()
    {
        h = 0;
        v = 0;
        _animator.SetBool("IsRunning", false);
        _animator.SetBool("RunningUp", false);
        _animator.SetBool("RunningDown", false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Death();
        }
    }

    async void Death()
    {
        _isAlive = false;
        _boxCollider2D.isTrigger =true;
        _animator.SetTrigger("PlayerDeath");
        SoundManager.Instance.PlaySound(SoundManager.Sound.Death);
        UiManager.Instance._lifeLine--;
        UiManager.Instance.UIDetails();
        if (UiManager.Instance._lifeLine <= 0)
        {
            await Task.Delay(1500);
            LevelManager.Instance.GameOver();
            await Task.Delay(1500);
            _boxCollider2D.isTrigger = false;
            _animator.SetTrigger("BackToIdle");
            //Reset Level -> Open GameOver
        }
        else
        {
            // Restart level
            await Task.Delay(1500);
            LevelManager.Instance.RestartLevel();
            await Task.Delay(1500);
            _boxCollider2D.isTrigger = false;
            _animator.SetTrigger("BackToIdle");
        }
    }
}
