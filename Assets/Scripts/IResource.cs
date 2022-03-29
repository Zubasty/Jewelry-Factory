using System;
using UnityEngine;

public interface IResource
{
    public event Action<IResource> Used;
    public void Install(Vector3 position, Transform parent);
}
