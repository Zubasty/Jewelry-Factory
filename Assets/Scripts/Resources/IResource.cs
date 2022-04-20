using System;
using UnityEngine;

public interface IResource
{
    public event Action<IResource> Installed;
    public Vector3 Position { get; }
    public Vector3 Angle { get; }
    public void SetParent(Transform parent);
    public void Move(Vector3 targetPosition, Vector3 angle);
    public void Install(Vector3 position, Vector3 angle);
    public void StartInstall(Mover mover, Vector3 targetPosition, Vector3 angle);
    public void Destroy();
}