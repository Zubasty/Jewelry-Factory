using System;
using System.Collections.Generic;
using UnityEngine;

public interface IResource
{
    public event Action<IResource> Installed;
    public Vector3 Position { get; }
    public Vector3 Angle { get; }
    public Quaternion DefaultAngle { get; }
    public void SetParent(Transform parent);
    public void Move(Vector3 targetPosition, Vector3 angle);
    public void Rotate(float x, float y, float z);
    public void Install(Vector3 position, Vector3 angle);
    public void StartInstall(Mover mover, List<Vector3> targetPositions, Vector3 angle);
    public void Destroy();
}