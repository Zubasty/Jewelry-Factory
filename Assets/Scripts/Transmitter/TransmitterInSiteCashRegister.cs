using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitterInSiteCashRegister : TransmitterAbstract
{
    protected override Vector3 ActualPosition => transform.position;

    protected override Vector3? ResultAngle => null;
}
