using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Caster))]
public class Player : MonoBehaviour
{
    [SerializeField] private Backpack _backpack;
    [SerializeField] private Arms _arms;

    private Mover _mover;
    private Caster _caster;
    private Money _money;
    private bool _isBusy;

    public int Money => _money.Value + _backpack.CountMoney;

    public void AddMoney(int count) => _money.AddMoney(count);

    public void Pay(int count) 
    {
        int takedMoney = _backpack.TakeMoney(count);
        _money.Pay(count - takedMoney);
    }

    public void AddMoney(int count, WadMoney wad)
    {
        _backpack.Add(wad);
        AddMoney(count);
    }

    public void TakeInBackpack(WadMoney wadMoney)
    {
        _backpack.Add(wadMoney);
    }

    public void TakeInArms(IResource resource)
    {
        _arms.Add(resource);
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _caster = GetComponent<Caster>();
        _money = new Money();
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

    private void OnFinished(TargetCaster target)
    {
        if (target.TryGetComponent(out IObjectInteractive objectInteractive))
        {
            _isBusy = true;
            objectInteractive.EndedInterection += OnEndedInteraction;
            objectInteractive.Interaction(this);
        }
    }

    private void OnEndedInteraction(IObjectInteractive owner)
    {
        owner.EndedInterection -= OnEndedInteraction;
        _isBusy = false;
    }
}
