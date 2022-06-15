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

    public event Action<Player> TakedWadsMoney;
    public event Action<Player> BoughtCashRegsiter;
    public event Action<Player> TakedResources;
    public event Action<Player> TakedMoney;
    public event Action StartedMove;
    public event Action StoppedMove;
    public event Action DroppedResources;

    public bool IsMove => _mover.IsMove;
    public bool HaveRocksInArms => _arms.HaveRocks;
    public int MoneyInBackpack => _backpack.CountMoney;
    public bool IsBusy { get; private set; }
    public IRock LastRockInArms => _arms.GetLastRock();

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
        _caster = GetComponent<Caster>();
        IsBusy = false;
    }

    private void OnEnable()
    {
        _mover.Finished += OnFinished;
        _mover.StartedMove += () => StartedMove?.Invoke();
        _mover.StoppedMove += () => StoppedMove?.Invoke();
        _arms.DroppedResources += () => DroppedResources?.Invoke();
    }

    private void OnDisable()
    {
        _mover.Finished -= OnFinished;
        _mover.StartedMove -= () => StartedMove?.Invoke();
        _mover.StoppedMove -= () => StoppedMove?.Invoke();
        _arms.DroppedResources -= () => DroppedResources?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            _caster.TryGetTarget(Camera.main.ScreenPointToRay(Input.mousePosition), out TargetCaster target) &&
            IsBusy == false)
        {
            _mover.SetTarget(target);
        }
    }

    public void StopForInteraction(IObjectInteractive objectInteractive) 
    {
        _mover.StopMove();
        StartInteraction(objectInteractive);
    }

    public void Buy(SiteCashRegister site)
    {
        site.TakeWadsMoney(_backpack);
        site.BoughtCashRegsiter += OnBoughtCashRegsiter;
    }

    public void TakeWadsMoney(StartupMoney listWadsMoney)
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

    private void StartInteraction(IObjectInteractive objectInteractive)
    {
        if (IsBusy)
            throw new Exception($"Игрок занят и не может начать взаимодействие с объектом {nameof(objectInteractive)}");

        IsBusy = true;
        objectInteractive.EndedInterection += OnEndedInteraction;
        objectInteractive.Interection(this);
    }

    private void OnFinished(TargetCaster target)
    {
        if (target.TryGetComponent(out IObjectInteractive objectInteractive))
        {
            StartInteraction(objectInteractive);
        }
    }

    private void OnEndedInteraction(IObjectInteractive owner)
    {
        owner.EndedInterection -= OnEndedInteraction;
        IsBusy = false;
    }

    public Queue<IRock> GiveResourcesFromArms(int count) => _arms.GiveRocks(count);

    public Queue<IRock> GiveResourcesFromArms() => _arms.GiveRocks();

}
