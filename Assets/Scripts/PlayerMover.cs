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

    private void InstallAngle()
    {
        Vector2 normalizedDirection = new Vector2(_target.Position.x - transform.position.x,
            _target.Position.z - transform.position.z).normalized;

        if (normalizedDirection.y < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,
                Mathf.Acos(normalizedDirection.x) * 360 / Mathf.PI, transform.rotation.z));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,
                180 - Mathf.Acos(normalizedDirection.x) * 360 / Mathf.PI, transform.rotation.z));
        }
    }

    private IEnumerator Moving()
    {
        InstallAngle();

        while (Vector3.Distance(transform.position, _target.Position) > _target.Distance)
        {
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _target.Position, _speed * Time.deltaTime));
            yield return null;
        }

        Finished?.Invoke(_target);
        _target = null;
        yield return null;
    }
}
