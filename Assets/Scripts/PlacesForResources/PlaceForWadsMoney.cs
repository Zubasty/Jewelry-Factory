using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceForWadsMoney : MonoBehaviour
{
    [SerializeField] private float _offsetY;
    [SerializeField] private List<PointForWadsMoney> _points;

    public int CountWads => _points.Sum(point => point.CountWads);

    private PointForWadsMoney PointForAdd
    {
        get
        {
            for(int i = 0; i < _points.Count - 1; i++)
            {
                if(_points[i+1].CountWads < _points[i].CountWads)
                {
                    return _points[i+1];
                }
            }
            return _points[0];
        }
    }

    private PointForWadsMoney PointForGive
    {
        get
        {
            for (int i = 0; i < _points.Count - 1; i++)
            {
                if (_points[i + 1].CountWads < _points[i].CountWads)
                {
                    return _points[i];
                }
            }

            return _points[_points.Count - 1];
        }
    }

    public WadMoney GiveWadMoney()
    {
        return PointForGive.GiveWadMoney();
    }

    public void Install(WadMoney wad)
    {
        PointForAdd.Add(wad);
    }

    public void StartAdd(WadMoney wad, Vector3 angle, float speed)
    {
        PointForAdd.StartInstall(wad, angle, speed);
    }

    private void Awake()
    {
        foreach (var point in _points)
        {
            point.Init(_offsetY);
        }
    }
}
