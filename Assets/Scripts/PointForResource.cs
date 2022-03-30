using System;
using System.Collections.Generic;
using UnityEngine;

public class PointForResource : MonoBehaviour
{
    private float _offsetY;
    private List<IResource> _resources;

    public int CountRecources => _resources.Count;

    public int Tier => _resources.Count - 1;

    public IResource GetResource(int index) => _resources[index];

    public IResource GetResource() => _resources[CountRecources - 1];

    public IResource GiveResource()
    {
        IResource resource = GetResource();
        LoseResource(resource);
        return resource;
    }

    public void Init(float offsetY)
    {
        _resources = new List<IResource>();
        _offsetY = offsetY;
    }

    public void Add(IResource resource)
    {
        _resources.Add(resource);
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + _offsetY * Tier, transform.position.z);
        resource.Install(newPosition, transform);
        resource.Used += LoseResource;
    }

    private void LoseResource(IResource resource)
    {
        resource.Used -= LoseResource;
        _resources.Remove(resource);
    }
}
