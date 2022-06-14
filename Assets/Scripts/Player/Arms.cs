using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResource))]
[RequireComponent(typeof(TransmitterPlace))]
[RequireComponent(typeof(Mover))]
public class Arms : MonoBehaviour
{
    private TransmitterPlace _transmitter;
    private Mover _mover;
    private PlaceForResource _place;

    public event Action TakedResources;
    public event Action DroppedResources;
    public event Action StartedTakeResources;

    public bool HaveRocks => _place.CountResources > 0;

    public Queue<IRock> GiveRocks(int count)
    {
        Queue<IRock> rocks = new Queue<IRock>();
        Queue<IResource> resources = _place.GiveResources(Mathf.Min(_place.CountResources, count));

        while (resources.Count > 0)
        {
            if (resources.Dequeue() is IRock rock)
                rocks.Enqueue(rock);
            else
                throw new InvalidCastException("Откуда в руках что-то кроме камней?");
        }

        DroppedResources?.Invoke();
        return rocks;
    }

    public Queue<IRock> GiveRocks() => GiveRocks(_place.CountResources);

    public IRock GetLastRock() 
    {
        IResource resource = _place.Resources[_place.Resources.Count - 1];

        if(resource is IRock rock)
            return rock;
        else
            throw new InvalidCastException("Откуда в руках что-то кроме камней?");
    }

    public void TakeResources<T>(PlaceAbstractFactory<T> place) where T : RockAbstract 
    {
        StartedTakeResources?.Invoke();
        _transmitter.Transfer(place.GiveAllResources());
    }

    private void Awake()
    {
        _place = GetComponent<PlaceForResource>();
        _transmitter = GetComponent<TransmitterPlace>();
        _mover = GetComponent<Mover>();
        _transmitter.Init(_mover, _place);
    }

    private void OnEnable()
    {
        _transmitter.Transferred += () => TakedResources?.Invoke();
    }

    private void OnDisable()
    {
        _transmitter.Transferred -= () => TakedResources?.Invoke();
    }
}
