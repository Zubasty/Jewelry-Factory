using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransmitterPoint))]
[RequireComponent(typeof(Mover))]
public class PlayerMoney : MonoBehaviour
{
    private TransmitterPoint _transmitter;
    private Mover _mover;
    private int _money;

    public event Action TakedAllMoney;
    public event Action TakedWadMoney;
    public event Action<int> SetedMoney;

    public int Money => _money;

    private void Awake()
    {
        _transmitter = GetComponent<TransmitterPoint>();
        _mover = GetComponent<Mover>();
        _transmitter.Init(_mover);
        _money = 0;
    }

    private void OnEnable()
    {
        _transmitter.Installed += OnInstalled;
        _transmitter.Transferred += () => TakedAllMoney?.Invoke();
    }

    private void OnDisable()
    {
        _transmitter.Installed -= OnInstalled;
        _transmitter.Transferred -= () => TakedAllMoney?.Invoke();
    }

    private void OnInstalled(IResource resource)
    {
        if(resource is WadMoney wad)
        {
            _money += wad.Amount;
            SetedMoney?.Invoke(_money);
            wad.Destroy();
            TakedWadMoney?.Invoke();
        }
        else
        {
            throw new InvalidCastException("ѕопытка вз€ть в качестве оплаты что-то кроме денег");
        }
    }

    public void Add(Queue<WadMoney> wads)
    {
        Queue<IResource> resources = new Queue<IResource>();

        while(wads.Count > 0)
            resources.Enqueue(wads.Dequeue());

        _transmitter.Transfer(resources);
    }
}
