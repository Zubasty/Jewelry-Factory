using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(TransmitterInSiteCashRegister))]
public class SiteCashRegister : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private int _price;

    private Mover _mover;
    private TransmitterInSiteCashRegister _transmitter;

    public event Action<IObjectInteractive> EndedInterection;
    public event Action<SiteCashRegister> BoughtCashRegsiter;

    public void Interection(Player player)
    {
        player.Buy(this);
        player.BoughtCashRegsiter += OnBoughtCashRegsiter;
    }

    public void TakeWadsMoney(Backpack backpack)
    {
        if(backpack.CountMoney >= _price)
        {
            _transmitter.Transfer(_mover, backpack.GiveWadsMoney(_price));
            _transmitter.Transferred += OnTransferred;
        }
        else
        {
            EndedInterection?.Invoke(this);
        }
    }

    private void OnTransferred()
    {
        BoughtCashRegsiter?.Invoke(this);
        _transmitter.Transferred -= OnTransferred;
    }

    private void OnBoughtCashRegsiter(Player player)
    {
        player.BoughtCashRegsiter -= OnBoughtCashRegsiter;
        EndedInterection?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnInstalled(WadMoney wad)
    {
        Destroy(wad.gameObject);
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _transmitter = GetComponent<TransmitterInSiteCashRegister>();
    }

    private void OnEnable()
    {
        _transmitter.Installed += OnInstalled;
    }

    private void OnDisable()
    {
        _transmitter.Installed -= OnInstalled;        
    }
}
