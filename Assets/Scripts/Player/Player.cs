using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Caster))]
public class Player : MonoBehaviour
{
    [SerializeField] private Backpack _backpack;

    private PlayerMover _mover;
    private Caster _caster;
    private bool _isBusy;

    public event Action<Player> TakedWadsMoney;
    public event Action<Player> BoughtCashRegsiter;

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
}
