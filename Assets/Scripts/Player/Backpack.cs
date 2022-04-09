using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlaceForWadsMoney))]
[RequireComponent(typeof(TransmitterInBackpack))]
[RequireComponent(typeof(Mover))]
public class Backpack : MonoBehaviour
{
    private TransmitterInBackpack _transmitter;
    private Mover _mover;
    private PlaceForWadsMoney _place;

    public event Action TakedWadsMoney;

    public int CountMoney => _place.CountMoney;

    public void TakeWadsMoney(ListWadsMoney wads)
    {
        _transmitter.Transfer(_mover, wads.Place.GiveAllWadsMoney());
    }

    public Queue<WadMoney> GiveWadsMoney(int price)
    {
        Queue<WadMoney> givenWads = new Queue<WadMoney>();

        while (givenWads.Sum(wad => wad.Amount) < price)
        {
            givenWads.Enqueue(_place.GiveWadMoney());
        }

        return givenWads;
    }

    private void OnTransferred()
    {
        TakedWadsMoney?.Invoke();
    }

    private void Awake()
    {
        _place = GetComponent<PlaceForWadsMoney>();
        _transmitter = GetComponent<TransmitterInBackpack>();
        _mover = GetComponent<Mover>();
        _transmitter.Init(_place);
    }

    private void OnEnable()
    {
        _transmitter.Transferred += OnTransferred;
    }

    private void OnDisable()
    {
        _transmitter.Transferred -= OnTransferred;
    }
}
