using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private TargetCaster _target;
    private Coroutine _moving;

    public event Action<TargetCaster> Finished;

    public void SetTarget(TargetCaster target)
    {
        _target = target;

        if(_moving != null)
        {
            StopCoroutine(_moving);
        }

        _moving = StartCoroutine(Moving());
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private IEnumerator Moving()
    {
        while(Vector3.Distance(transform.position, _target.Position) > _target.Distance)
        {
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _target.Position, _speed * Time.deltaTime));
            yield return null;
        }
        Finished?.Invoke(_target);
        _target = null;
        yield return null;
    }
}
