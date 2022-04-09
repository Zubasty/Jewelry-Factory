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

    public int CountMoney => _points.Sum(point => point.CountMoney);

    public Vector3 ActualPosition => PointForAdd.ActualPosition;

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

    public void Init(List<WadMoney> wads)
    {

    }

    public WadMoney GiveWadMoney()
    {
        return PointForGive.GiveWadMoney();
    }

    public Queue<WadMoney> GiveAllWadsMoney()
    {
        Queue<WadMoney> givenWads = new Queue<WadMoney>();

        while (CountWads > 0)
        {
            givenWads.Enqueue(GiveWadMoney());
        }

        return givenWads;
    }

    public void Install(WadMoney wad)
    {
        PointForAdd.Add(wad);
    }

    public void StartAdd(WadMoney wad)
    {
        PointForAdd.AddInQueue(wad);
    }

    private void Awake()
    {
        foreach (var point in _points)
        {
            point.Init(_offsetY);
        }
    }
}
