using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransmitterPlace))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(PlaceForResource))]
public class PlaceForPay : MonoBehaviour, IObjectInteractive
{
    private PlaceForResource _place;
    private TransmitterPlace _transmitter;
    private Mover _mover;

    public event Action<IObjectInteractive> EndedInterection;

    private void Awake()
    {
        _transmitter = GetComponent<TransmitterPlace>();
        _mover = GetComponent<Mover>();
        _place = GetComponent<PlaceForResource>();
        _transmitter.Init(_mover, _place);
    }

    public void Interection(Player player)
    {
        if(_place.CountResources > 0)
        {
            player.TakeMoney(this);
            player.TakedMoney += OnTakedMoney;
        }
        else
        {
            EndedInterection?.Invoke(this);
        }
    }

    public void Pay(Queue<WadMoney> wadsMoney)
    {
        Queue<IResource> resources = new Queue<IResource>();

        while(wadsMoney.Count > 0)
        {
            resources.Enqueue(wadsMoney.Dequeue());
        }

        _transmitter.Transfer(resources);
    }

    public Queue<WadMoney> GiveResources()
    {
        Queue<WadMoney> wadsMoney = new Queue<WadMoney>();
        Queue<IResource> resources = _place.GiveResources();

        while(resources.Count > 0)
        {
            if (resources.Dequeue() is WadMoney wad)
                wadsMoney.Enqueue(wad);
            else
                throw new InvalidCastException("ќткуда в кассе что-то кроме денег?");
        }

        return wadsMoney;
    }

    private void OnTakedMoney(Player player)
    {
        player.TakedMoney -= OnTakedMoney;
        EndedInterection?.Invoke(this);
    }
}
