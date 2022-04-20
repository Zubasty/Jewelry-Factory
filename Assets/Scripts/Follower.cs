using UnityEngine;

public class  Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _offset;

    private void Awake()
    {
        _offset =_target.transform.position - transform.position;
    }

    private void Update()
    {
        transform.position = _target.transform.position - _offset;
    }
}
