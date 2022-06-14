using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransmitterWithMiddlePoint : MonoBehaviour, ITransmitter
{
    private Mover _mover;
    private float _yPosition;
    private Transform _firstPoint;
    private Queue<IResource> _resources;
    private List<IResource> _resourcesMoving;

    public event Action<IResource> Installed;
    public event Action Transferred;

    private List<Vector3> ActualPositions
    {
        get
        {
            List<Vector3> result = new List<Vector3>();
            result.Add(new Vector3(_firstPoint.position.x, _yPosition, _firstPoint.position.z));
            result.Add(transform.position);
            return result;
        }
    }

    public bool IsFree => _resourcesMoving.Count == 0 && _resources.Count == 0;

    private void Awake()
    {
        _resources = new Queue<IResource>();
        _resourcesMoving = new List<IResource>();
    }

    private void StartMoveNextResource()
    {
        if (_resources.Count > 0)
        {
            IResource resource = _resources.Dequeue();
            _resourcesMoving.Add(resource);
            resource.Installed += EndTransfer;
            List<Vector3> actualPositions = ActualPositions;
            resource.StartInstall(_mover, actualPositions, resource.Angle);
        }
    }

    private void OnInstalledPosition(IResource resource)
    {
        if(resource.Position != transform.position)
        {
            StartMoveNextResource();
        }
    }

    public void Init(Mover mover, float yPosition, Transform firstPosition)
    {
        _mover = mover;
        _mover.InstalledInPosition -= OnInstalledPosition;
        _mover.InstalledInPosition += OnInstalledPosition;
        _yPosition = yPosition;
        _firstPoint = firstPosition;
    }

    public void Transfer(Queue<IResource> resources)
    {
        while (resources.Count > 0)
            _resources.Enqueue(resources.Dequeue());
        StartMoveNextResource();
    }

    private void EndTransfer(IResource resource)
    {
        _resourcesMoving.Remove(resource);
        resource.Installed -= EndTransfer;
        Installed?.Invoke(resource);

        if (IsFree)
        {
            Transferred?.Invoke();
        }
    }
}
