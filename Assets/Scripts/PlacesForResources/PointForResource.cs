using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointForResource : MonoBehaviour
{
    private List<IResource> _resources;
    private List<IResource> _queueResources;
    private float _offsetY;

    public Vector3 ActualPosition => new Vector3(transform.position.x,
            transform.position.y + _offsetY * CountResourcesWithQueue, transform.position.z);

    public Vector3 ActualPositionWithoutQueue => new Vector3(transform.position.x,
        transform.position.y + _offsetY * CountResources, transform.position.z);

    public int CountResources => _resources.Count;

    public int CountResourcesWithQueue => _resources.Count + _queueResources.Count;

    public IReadOnlyList<IResource> Resources => _resources;

    public IResource GiveResource()
    {
        IResource resource = _resources[_resources.Count - 1];
        _resources.Remove(resource);
        resource.SetParent(null);
        return resource;
    }

    public void Init(float offsetY)
    {
        _offsetY = offsetY;
        _resources = new List<IResource>();
        _queueResources = new List<IResource>(); 
    }

    public void Add(IResource resource) => Add(resource, ActualPosition);

    public void AddInQueue(IResource resource)
    {
        Vector3 position = ActualPosition;
        resource.Installed += OnInstalled;
        _queueResources.Add(resource);
    }

    private Vector3 GetPosition(int index) => new Vector3(transform.position.x,
            transform.position.y + _offsetY * index, transform.position.z);

    private void OnInstalled(IResource resource)
    {
        resource.Installed -= OnInstalled;
        _queueResources.Remove(resource);
        Add(resource, resource.Position);
    }

    private void Add(IResource resource, Vector3 position)
    {
        _resources.Add(resource);
        resource.SetParent(transform);
        MoveAllResources();
    }

    private void MoveAllResources()
    {
        for(int i = 0; i<_resources.Count; i++)
            _resources[i].Move(GetPosition(i), _resources[i].Angle);
    }
}
