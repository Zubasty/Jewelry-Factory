using UnityEngine;

public class TargetCaster : MonoBehaviour
{
    [SerializeField] private float _distanceForInteraction;

    private Vector3 _targetPosition;

    public Vector3 Position => _targetPosition;

    public float Distance => _distanceForInteraction;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    public void SetPosition(Vector3 newPosition)
    {
        _targetPosition = newPosition;
    }
}