using System.Collections.Generic;
using UnityEngine;

public class TransmitterPoint : TransmitterAbstract
{
    protected override List<Vector3> ActualPositions
    {
        get
        {
            List<Vector3> result = new List<Vector3>();
            result.Add(transform.position);
            return result;
        }
    }
}
