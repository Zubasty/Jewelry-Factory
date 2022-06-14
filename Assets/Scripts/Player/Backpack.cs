using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResource))]
[RequireComponent(typeof(TransmitterPlaceFirstRow))]
[RequireComponent(typeof(Mover))]
public class Backpack : MonoBehaviour
{
    [SerializeField] private Transform _boneParent;

    private TransmitterPlaceFirstRow _transmitter;
    private Mover _mover;
    private PlaceForResource _place;

    public event Action TakedWadsMoney;

    public Vector3 MaxPosition
    {
        get;
        private set;
    }

    public PointForResource FirstPoint => _place.FirstPoint;

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
        _mover = GetComponent<Mover>();
        _transmitter = GetComponent<TransmitterPlaceFirstRow>();
        _transmitter.Init(_mover, _place);
    }

    private void OnEnable()
    {
        _transmitter.Transferred += OnTransfferd;
    }

    private void OnDisable()
    {
        _transmitter.Transferred -= OnTransfferd;
    }

    public void OnTransfferd()
    {
        TakedWadsMoney?.Invoke();
        transform.parent = _boneParent;
        MaxPosition = new Vector3(_place.FirstPoint.transform.position.x, _place.ActualPosition.y, _place.FirstPoint.transform.position.z);
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
