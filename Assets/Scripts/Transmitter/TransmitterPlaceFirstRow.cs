using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitterPlaceFirstRow : TransmitterAbstract
{
    private PlaceForResource _place;

    protected override List<Vector3> ActualPositions
    {
        get
        {
            List<Vector3> result = new List<Vector3>();
            result.Add(new Vector3(_place.FirstPoint.transform.position.x, _place.ActualPosition.y, _place.FirstPoint.transform.position.z));
            result.Add(_place.ActualPosition);
            return result;
        }
    }
    private void OnEnable()
    {
        ActionBeforeStartInstall += Add;
    }

    private void OnDisable()
    {
        ActionBeforeStartInstall -= Add;
    }

    public void Init(Mover mover, PlaceForResource place)
    {
        Init(mover);
        _place = place;
    }

    private void Add(IResource resource)
    {
        _place.StartAdd(resource);
    }
}
