using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResource))]
[RequireComponent(typeof(TransmitterPlace))]
[RequireComponent(typeof(Mover))]
public class Backpack : MonoBehaviour
{
    private TransmitterPlace _transmitter;
    private Mover _mover;
    private PlaceForResource _place;

    public event Action TakedWadsMoney;

    public int CountMoney
    {
        get
        {
            int count = 0;

            for(int i = 0; i < _place.Resources.Count; i++)
            {
                if(_place.Resources[i] is WadMoney wad)
                    count += wad.Amount;
                else
                    throw new InvalidCastException("Откуда в рюкзаке что-то кроме пачек денег?");
            }

            return count;
        }
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
        _transmitter.Transferred += () => TakedWadsMoney?.Invoke();
    }

    private void OnDisable()
    {
        _transmitter.Transferred -= () => TakedWadsMoney?.Invoke();
    }

    public void TakeWadsMoney(ListWadsMoney listWadsMoney) => _transmitter.Transfer(listWadsMoney.Place.GiveResources());

    public Queue<IResource> GiveWadsMoney(int price)
    {
        Queue<IResource> givenWads = new Queue<IResource>();
        int money = 0;

        while (money < price)
        {
            IResource resource = _place.GiveResources(1).Dequeue();

            if(resource is WadMoney wad)
            {
                givenWads.Enqueue(resource);
                money += wad.Amount;
            }
            else
            {
                throw new InvalidCastException("Откуда в рюкзаке что-то кроме пачек денег?");
            }
        }

        return givenWads;
    }
}
