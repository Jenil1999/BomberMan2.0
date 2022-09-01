using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCreator : MonoBehaviour
{
    // AnimationCreator
    SpriteRenderer _spriteRenderer;

    [Header("Set Sprites For Animation")]
    public Sprite _idleSprite;

    public Sprite[] _spritesForAnimations;

    [Header("Set Animation Time")]
    public float _animationTime;

    public int _animationFrame;

    [Header("Animation Type")]
    public bool _animateInLoop;

    public bool _showIdleSprite;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), _animationTime, _animationTime);
    }

    void NextFrame()
    {
        if (_animateInLoop && _animationFrame >= _spritesForAnimations.Length)
        {
            _animationFrame = 0;
        }
        if (_idleSprite)
        {
            _spriteRenderer.sprite = _idleSprite;
        }
        else if (_animationFrame >= 0 && _animationFrame < _spritesForAnimations.Length)
        {
            _spriteRenderer.sprite = _spritesForAnimations[_animationFrame];
        }
    }
}
