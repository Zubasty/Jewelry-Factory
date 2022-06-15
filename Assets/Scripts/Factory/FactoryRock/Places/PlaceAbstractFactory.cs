using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResource))]
[RequireComponent(typeof(TransmitterPlace))]
[RequireComponent(typeof(Mover))]
public abstract class PlaceAbstractFactory<T> : MonoBehaviour, IObjectInteractive where T : RockAbstract
{
    private PlaceForResource _place;
    private TransmitterPlace _transmitter;
    private Mover _mover;

    public event Action<IObjectInteractive> EndedInterection;

    public bool IsReady => _place.CountResources > 0;

    private void Awake()
    {
        _place = GetComponent<PlaceForResource>();
        _mover = GetComponent<Mover>();
        _transmitter = GetComponent<TransmitterPlace>();
        _transmitter.Init(_mover, _place);
    }

    public void Add(T rock)
    {
        Queue<IResource> queue = new Queue<IResource>();
        queue.Enqueue(rock);
        _transmitter.Transfer(queue);
    }
    
    public void Install(T rock)
    {
        _place.Install(rock);
    }

    public Queue<IResource> GiveAllResources() => _place.GiveResources();

    public void Interection(Player player)
    {
        if (CanInterection(player))
        {
            player.TakeResources(this);
            player.TakedResources += OnTakedResources;
        }
        else
        {
            EndedInterection?.Invoke(this);
        }
    }

    public bool CanInterection(Player player) => _place.CountResources > 0 && (player.HaveRocksInArms == false || player.LastRockInArms is T);

    private void OnTakedResources(Player player)
    {
        player.TakedResources -= OnTakedResources;
        EndedInterection?.Invoke(this);
    }
}
