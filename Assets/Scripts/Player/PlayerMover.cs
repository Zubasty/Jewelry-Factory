using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _speedRotateWithMove;

    private Rigidbody _rigidbody;
    private Coroutine _moving;

    public event Action<TargetCaster> Finished;
    public event Action StoppedMove;
    public event Action StartedMove;

    public bool IsMove { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        IsMove = false;
    }

    public void SetTarget(TargetCaster target)
    {
        if (_moving != null)
        {
            StopCoroutine(_moving);
            _moving = StartCoroutine(Moving(target));
        }
        else
        {
            _moving = StartCoroutine(RotatingMoving(target));
        }    
    }

    public void StopMove()
    {
        if(_moving != null)
            StopCoroutine(_moving);

        _moving = null;
        IsMove = false;
        StoppedMove?.Invoke();
    }

    private Vector3 GetResultAngle(TargetCaster target)
    {
        Vector2 direction = new Vector2(target.Position.x - transform.position.x,
            target.Position.z - transform.position.z);
        Vector2 normalizedDirection = direction.normalized;

        if(normalizedDirection.y < 0)
            return new Vector3(transform.rotation.x, 90 + Mathf.Acos(normalizedDirection.x) * 180 / Mathf.PI, transform.rotation.z);
        else
            return new Vector3(transform.rotation.x, 90 - Mathf.Acos(normalizedDirection.x) * 180 / Mathf.PI, transform.rotation.z);
    }

    private void Rotate(TargetCaster target, float speed)
    {
        Quaternion resultAngle = Quaternion.Euler(GetResultAngle(target));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, resultAngle, speed * Time.deltaTime);
    }

    private IEnumerator RotatingMoving(TargetCaster target)
    {
        yield return StartCoroutine(Rotating(target));
        _moving = StartCoroutine(Moving(target));
        yield return _moving;
    }

    private IEnumerator Rotating(TargetCaster target)
    {
        while(Mathf.DeltaAngle(transform.eulerAngles.y, GetResultAngle(target).y) != 0)
        {
            Rotate(target, _speedRotate);
            yield return null;
        }
    }

    private IEnumerator Moving(TargetCaster target)
    {
        IsMove = true;
        StartedMove?.Invoke();

        while (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
            new Vector2(target.Position.x, target.Position.z)) > target.Distance)
        {
            Rotate(target, _speedRotateWithMove);
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, transform.position + transform.forward, _speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, target.Position))
                _rigidbody.MovePosition(target.Position);
            else
                _rigidbody.MovePosition(targetPosition);
            yield return null;
        }

        Finished?.Invoke(target);
        StopMove();
    }
}
