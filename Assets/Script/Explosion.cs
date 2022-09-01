using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public AnimationCreator Start;
    public AnimationCreator Middle;
    public AnimationCreator End;

    public void DestroyAfter(float _seconds)
    {
        Destroy(gameObject, _seconds);
    }

    public void SetDirection(Vector2 _direction)
    {

        float _angle = Mathf.Atan2(_direction.y, _direction.x);
        transform.rotation = Quaternion.AngleAxis(_angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void SetActiveSprites(AnimationCreator _CheckSpriteRenderer)
    {
        Start.enabled = _CheckSpriteRenderer == Start;
        Middle.enabled = _CheckSpriteRenderer == Middle;
        End.enabled = _CheckSpriteRenderer == End;
    }

}
