using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitterInBackpack : TransmitterAbstract
{
    [SerializeField] private Vector3 _resultAngle;

    private PlaceForWadsMoney _place;

    protected override Vector3 ActualPosition => _place.ActualPosition;

    protected override Vector3? ResultAngle => _resultAngle + transform.rotation.eulerAngles;

    public void Init(PlaceForWadsMoney place)
    {
        _place = place;
    }

    private void Add(WadMoney wad)
    {
        _place.StartAdd(wad);
    }

    private void OnEnable()
    {
        ActionAfterStartInstall += Add;
    }

    private void OnDisable()
    {
        ActionAfterStartInstall -= Add;
    }
}
