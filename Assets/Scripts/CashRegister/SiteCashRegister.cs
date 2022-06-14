using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(TransmitterWithMiddlePoint))]
public class SiteCashRegister : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private int _price;
    [SerializeField] private CashRegister _cashRegisterTemplate;
    [SerializeField] private ParticleDestructible _particleTemplate;
    [SerializeField] private PlaceForPay _placeForPay;

    private Mover _mover;
    private TransmitterWithMiddlePoint _transmitter;

    public event Action<IObjectInteractive> EndedInterection;
    public event Action<SiteCashRegister> BoughtCashRegsiter;
    public event Action<CashRegister> MakedCashRegister;
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
        _transmitter = GetComponent<TransmitterWithMiddlePoint>();
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

    public bool CanInterection(Player player) => player.MoneyInBackpack >= Price;

    public void Interection(Player player)
    {
        player.Buy(this);
        player.BoughtCashRegsiter += OnBoughtCashRegsiter;
    }

    public void TakeWadsMoney(Backpack backpack)
    {
        if(backpack.CountMoney >= Price)
        {
            _transmitter.Init(_mover, backpack.MaxPosition.y, backpack.FirstPoint.transform);
            _transmitter.Transfer(backpack.GiveWadsMoney(Price));
        }
        else
        {
            EndedInterection?.Invoke(this);
        }           
    }

    private void OnBoughtCashRegsiter(Player player)
    {
        player.BoughtCashRegsiter -= OnBoughtCashRegsiter;
        EndedInterection?.Invoke(this);
        CashRegister cashRegister = Instantiate(_cashRegisterTemplate, new Vector3(transform.position.x, _cashRegisterTemplate.transform.position.y, 
            transform.position.z), _cashRegisterTemplate.transform.rotation);
        cashRegister.Init(_placeForPay);
        MakedCashRegister?.Invoke(cashRegister);
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
