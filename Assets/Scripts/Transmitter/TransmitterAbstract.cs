using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransmitterAbstract : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private bool _isTakeAngle;
    [SerializeField] private Vector3 _resultAngle;

    private Mover _mover;
    private float _timeBeforeStartMove;
    private Queue<IResource> _resources;
    private List<IResource> _resourcesMoving;

    protected Action<IResource> ActionAfterStartInstall;

    public event Action<IResource> Installed;
    public event Action Transferred;

    protected abstract Vector3 ActualPosition { get; }

    protected Vector3? Angle
    {
        get
        {
            if (_isTakeAngle)
                return _resultAngle + transform.rotation.eulerAngles;
            else
                return null;
        }
    } 

    protected float Delay => _delay;

    public bool IsFree => _resources.Count == 0 && _resourcesMoving.Count == 0;

    private void Awake()
    {
        _resources = new Queue<IResource>();
        _resourcesMoving = new List<IResource>();
        _timeBeforeStartMove = 0;
    }

    private void Update()
    {
        _timeBeforeStartMove = Mathf.Max(0, _timeBeforeStartMove - Time.deltaTime);

        if(_timeBeforeStartMove == 0)
        {
            if (_resources.Count > 0)
            {
                IResource resource = _resources.Dequeue();
                resource.StartInstall(_mover, ActualPosition, Angle.HasValue ? 
                    Angle.Value : resource.Angle);
                ActionAfterStartInstall?.Invoke(resource);
                resource.Installed += EndTransfer;
                _resourcesMoving.Add(resource);
                _timeBeforeStartMove = _delay;
            }
        }

    }

    public void Init(Mover mover) => _mover = mover;

    public void Transfer(Queue<IResource> resources)
    {
        while(resources.Count > 0)
            _resources.Enqueue(resources.Dequeue());
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
