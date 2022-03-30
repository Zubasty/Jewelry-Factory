using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RockAbstract : MonoBehaviour, IResource
{
    public event Action<IResource> Used;

    public void Install(Vector3 position, Transform parent)
    {
        transform.parent = parent;
        transform.position = position;
    }
}
