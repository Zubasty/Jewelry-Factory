using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private TargetCaster _target;
    private Coroutine _moving;

    public event Action<TargetCaster> Finished;

    public bool IsMove => _target != null;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTarget(TargetCaster target)
    {
        _target = target;

        if (_moving != null)
            StopCoroutine(_moving);

        _moving = StartCoroutine(Moving());
    }

    private void InstallAngle()
    {
        Vector2 direction = new Vector2(_target.Position.x - transform.position.x,
            _target.Position.z - transform.position.z);
        Vector2 normalizedDirection = direction.normalized;

        if(normalizedDirection.y < 0)
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,
                90 + Mathf.Acos(normalizedDirection.x) * 180 / Mathf.PI, transform.rotation.z));
        else
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,
                90 - Mathf.Acos(normalizedDirection.x) * 180 / Mathf.PI, transform.rotation.z));
    }

    private IEnumerator Moving()
    {
        InstallAngle();

        while (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
            new Vector2(_target.Position.x, _target.Position.z)) > _target.Distance)
        {
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _target.Position, _speed * Time.deltaTime));
            yield return null;
        }

        Finished?.Invoke(_target);
        _target = null;
        yield return null;
    }
}
