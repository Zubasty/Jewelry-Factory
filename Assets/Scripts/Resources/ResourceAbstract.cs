using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceAbstract : MonoBehaviour, IResource
{
    public event Action<IResource> Installed;

    public Vector3 Position => transform.position;
    public Vector3 Angle => transform.rotation.eulerAngles;
    public Quaternion DefaultAngle { get; private set; }

    private void Start()
    {
        DefaultAngle = transform.rotation;
    }

    public void Move(Vector3 targetPosition, Vector3 angle)
    {
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(angle);
    }

    public void Install(Vector3 position, Vector3 angle)
    {
        Move(position, angle);
        transform.rotation = Quaternion.Euler(angle);
        Installed?.Invoke(this);
    }

    public void StartInstall(Mover mover, List<Vector3> targetPositions, Vector3 angle) => mover.StartMove(this, targetPositions, angle);

    public void SetParent(Transform parent) => transform.parent = parent;

    public void Destroy() => Destroy(gameObject);

    public void Rotate(float x, float y, float z) => transform.Rotate(x, y, z);
}
