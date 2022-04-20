using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(TransmitterPoint))]
public class SiteCashRegister : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private int _price;
    [SerializeField] private CashRegister _cashRegister;
    [SerializeField] private ParticleDestructible _particleTemplate;

    private Mover _mover;
    private TransmitterPoint _transmitter;

    public event Action<IObjectInteractive> EndedInterection;
    public event Action<SiteCashRegister> BoughtCashRegsiter;
    public event Action<int> SetedPrice;

    public int Price
    {
        get => _price;
        private set 
        {
            _price = Mathf.Max(0, value);
            SetedPrice?.Invoke(_price);
        }
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _transmitter = GetComponent<TransmitterPoint>();
        _transmitter.Init(_mover);
        _cashRegister.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _transmitter.Transferred += () => BoughtCashRegsiter?.Invoke(this);
        _transmitter.Installed += OnInstalled;
    }

    private void OnDisable()
    {
        _transmitter.Transferred -= () => BoughtCashRegsiter?.Invoke(this);
        _transmitter.Installed -= OnInstalled;
    }

    public void Interection(Player player)
    {
        player.Buy(this);
        player.BoughtCashRegsiter += OnBoughtCashRegsiter;
    }

    public void TakeWadsMoney(Backpack backpack)
    {
        if(backpack.CountMoney >= Price)
            _transmitter.Transfer(backpack.GiveWadsMoney(Price));
        else
            EndedInterection?.Invoke(this);
    }

    private void OnBoughtCashRegsiter(Player player)
    {
        player.BoughtCashRegsiter -= OnBoughtCashRegsiter;
        EndedInterection?.Invoke(this);
        _cashRegister.gameObject.SetActive(true);
        Instantiate(_particleTemplate, transform.position, _particleTemplate.transform.rotation);
        Destroy(gameObject);
    }

    private void OnInstalled(IResource resource)
    {
        if(resource is WadMoney wad)
        {
            Price -= wad.Amount;
            wad.Destroy();
        }
        else
        {
            throw new InvalidCastException("ѕопытка оплатить кассу чем-то кроме денег");
        }
    }
}
