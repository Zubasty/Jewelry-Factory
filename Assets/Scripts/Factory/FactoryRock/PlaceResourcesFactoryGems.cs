using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResource))]
[RequireComponent(typeof(TransmitterPlace))]
[RequireComponent(typeof(Mover))]
public class PlaceResourcesFactoryGems : MonoBehaviour, IObjectInteractive
{
    private PlaceForResource _place;
    private TransmitterPlace _transmitter;
    private Mover _mover;

    public event Action<IObjectInteractive> EndedInterection;

    public bool HaveResources => _place.CountResources > 0;

    public void Interection(Player player)
    {
        if(player.HaveRocksInArms && player.LastRockInArms is Rock)
        {
            Queue<IResource> resources = new Queue<IResource>();
            Queue<IRock> rocks = player.GiveResourcesFromArms();

            while(rocks.Count > 0)
            {
                resources.Enqueue(rocks.Dequeue());
            }

            _transmitter.Transfer(resources);
        }
        else
        {
            EndedInterection?.Invoke(this);
        }
    }

    public IResource GiveResource()
    {
        return _place.GiveResources(1).Dequeue();
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
        _transmitter.Transferred += () => EndedInterection?.Invoke(this);
    }

    private void OnDisable()
    {
        _transmitter.Transferred -= () => EndedInterection?.Invoke(this);    
    }
}
