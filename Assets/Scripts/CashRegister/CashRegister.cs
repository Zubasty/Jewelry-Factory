using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResource))]
[RequireComponent(typeof(TransmitterPlace))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(SizeAnimation))]
public class CashRegister : MonoBehaviour, IObjectInteractive
{
    private PlaceForPay _placeForPay;
    private SizeAnimation _animation;
    private PlaceForResource _place;
    private TransmitterPlace _transmitter;
    private Mover _mover;
    private bool _isPreparing;

    public event Action<IObjectInteractive> EndedInterection;

    public int CountGems => _place.CountResources;
    public bool IsFull => _place.CountResources == _place.CountPoints;
    public bool IsReady => IsFull == false && _isPreparing == false;

    private void Awake()
    {
        _place = GetComponent<PlaceForResource>();
        _transmitter = GetComponent<TransmitterPlace>();
        _mover = GetComponent<Mover>();
        _animation = GetComponent<SizeAnimation>();
        _transmitter.Init(_mover, _place);
    }

    private void OnEnable()
    {
        _transmitter.Transferred += () => EndedInterection?.Invoke(this);
        _animation.EndedAnimation += () => _isPreparing = false;
    }

    private void Start()
    {
        _animation.StartAnimation(transform);
    }

    private void OnDisable()
    {
        _transmitter.Transferred -= () => EndedInterection?.Invoke(this);
        _animation.EndedAnimation -= () => _isPreparing = false;
    }

    public bool CanInterection(Player player) => IsFull == false && _isPreparing == false && player.HaveRocksInArms && player.LastRockInArms is Gem;

    public void Init(PlaceForPay placeForPay)
    {
        _placeForPay = placeForPay;
        _placeForPay.transform.parent = transform;
        _isPreparing = true;
    }

    public void Interection(Player player)
    {
        if (IsFull == false && player.HaveRocksInArms && player.LastRockInArms is Gem)
        {
            Queue<IResource> resources = new Queue<IResource>();
            Queue<IRock> gems = player.GiveResourcesFromArms(_place.CountPoints - _place.CountResources);

            while (gems.Count > 0)
            {
                resources.Enqueue(gems.Dequeue());
            }

            _transmitter.Transfer(resources);
        }
        else
        {
            EndedInterection?.Invoke(this);
        }
    }

    public void Pay(Queue<WadMoney> wadsMoney) => _placeForPay.Pay(wadsMoney);

    public Gem GiveResource()
    {
        if(_place.GiveResources(1).Dequeue() is Gem gem)
        {
            return gem;
        }
        else
        {
            throw new InvalidCastException("Откуда на кассе что-то кроме драгоценных камней?");
        }
    }
}
