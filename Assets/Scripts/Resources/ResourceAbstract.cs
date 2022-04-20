using System;
using UnityEngine;

public abstract class ResourceAbstract : MonoBehaviour, IResource
{
    public event Action<IResource> Installed;

    public Vector3 Position => transform.position;

    public Vector3 Angle => transform.rotation.eulerAngles;

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

    public void StartInstall(Mover mover, Vector3 targetPosition, Vector3 angle) => mover.StartMove(this, targetPosition, angle);

    public void SetParent(Transform parent) => transform.parent = parent;

    public void Destroy() => Destroy(gameObject);
}
