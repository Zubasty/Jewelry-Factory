using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransmitterPlace))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(PlaceForResource))]
public class StandFactoryGem : MonoBehaviour
{
    private TransmitterPlace _transmitter;
    private Mover _mover;
    private PlaceForResource _place;

    public void Add(IResource resource)
    {
        Queue<IResource> queue = new Queue<IResource>();
        queue.Enqueue(resource);
        _transmitter.Transfer(queue);
    }

    public IResource GiveResource() => _place.GiveResources(1).Dequeue();

    private void Awake()
    {
        _transmitter = GetComponent<TransmitterPlace>();
        _mover = GetComponent<Mover>();
        _place = GetComponent<PlaceForResource>();
        _transmitter.Init(_mover, _place);
    }
}
