using UnityEngine;

public class TransmitterPoint : TransmitterAbstract
{
    protected override Vector3 ActualPosition => transform.position;
}
