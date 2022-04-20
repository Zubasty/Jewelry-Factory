using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Caster))]
public class Player : MonoBehaviour
{
    [SerializeField] private Backpack _backpack;
    [SerializeField] private Arms _arms;
    [SerializeField] private PlayerMoney _money;

    private PlayerMover _mover;
    private Caster _caster;
    private bool _isBusy;

    public event Action<Player> TakedWadsMoney;
    public event Action<Player> BoughtCashRegsiter;
    public event Action<Player> TakedResources;
    public event Action<Player> TakedMoney;

    public bool IsMove => _mover.IsMove;

    public bool HaveRocksInArms => _arms.HaveRocks;

    public IRock LastRockInArms => _arms.GetLastRock();

    public Queue<IRock> GiveResourcesFromArms(int count) => _arms.GiveRocks(count);

    public Queue<IRock> GiveResourcesFromArms() => _arms.GiveRocks();

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
        _caster = GetComponent<Caster>();
        _isBusy = false;
    }

    private void OnEnable()
    {
        _mover.Finished += OnFinished;
    }

    private void OnDisable()
    {
        _mover.Finished -= OnFinished;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            _caster.TryGetTarget(Camera.main.ScreenPointToRay(Input.mousePosition), out TargetCaster target) &&
            _isBusy == false)
        {
            _mover.SetTarget(target);
        }
    }

    public void Buy(SiteCashRegister site)
    {
        site.TakeWadsMoney(_backpack);
        site.BoughtCashRegsiter += OnBoughtCashRegsiter;
    }

    public void TakeWadsMoney(ListWadsMoney listWadsMoney)
    {
        _backpack.TakeWadsMoney(listWadsMoney);
        _backpack.TakedWadsMoney += OnTakedWadsMoney;
    }

    public void TakeResources<T>(PlaceAbstractFactory<T> place) where T : RockAbstract
    {
        _arms.TakeResources(place);
        _arms.TakedResources += OnTakedResources;
    }

    public void TakeMoney(PlaceForPay place)
    {
        _money.Add(place.GiveResources());
        _money.TakedAllMoney += OnTakedMoney;
    }

    private void OnTakedMoney()
    {
        TakedMoney?.Invoke(this);
        _money.TakedAllMoney -= OnTakedMoney;
    }

    private void OnTakedResources()
    {
        _arms.TakedResources -= OnTakedResources;
        TakedResources?.Invoke(this);
    }

    private void OnBoughtCashRegsiter(SiteCashRegister site)
    {
        site.BoughtCashRegsiter -= OnBoughtCashRegsiter;
        BoughtCashRegsiter?.Invoke(this);
    }

    private void OnTakedWadsMoney()
    {
        _backpack.TakedWadsMoney -= OnTakedWadsMoney;
        TakedWadsMoney?.Invoke(this);
    }

    private void OnFinished(TargetCaster target)
    {
        if (target.TryGetComponent(out IObjectInteractive objectInteractive))
        {
            _isBusy = true;
            objectInteractive.EndedInterection += OnEndedInteraction;
            objectInteractive.Interection(this);
        }
    }

    private void OnEndedInteraction(IObjectInteractive owner)
    {
        owner.EndedInterection -= OnEndedInteraction;
        _isBusy = false;
    }
}
