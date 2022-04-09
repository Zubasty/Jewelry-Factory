using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointForWadsMoney : MonoBehaviour
{
    private List<WadMoney> _wadsMoney;
    private List<WadMoney> _queueWadsMoney;
    private float _offsetY;

    public Vector3 ActualPosition => new Vector3(transform.position.x,
            transform.position.y + _offsetY * CountWads, transform.position.z);

    public int CountWads => _wadsMoney.Count + _queueWadsMoney.Count;

    public int CountMoney => _wadsMoney.Sum(wad => wad.Amount) + _queueWadsMoney.Sum(wad => wad.Amount);

    public WadMoney GiveWadMoney()
    {
        WadMoney wadMoney = _wadsMoney[_wadsMoney.Count - 1];
        _wadsMoney.Remove(wadMoney);
        wadMoney.transform.parent = null;
        return wadMoney;
    }

    public void Init(float offsetY)
    {
        _offsetY = offsetY;
        _wadsMoney = new List<WadMoney>();
        _queueWadsMoney = new List<WadMoney>(); 
    }

    public void Add(WadMoney wadMoney) => Add(wadMoney, ActualPosition);

    public void AddInQueue(WadMoney wad)
    {
        Vector3 position = ActualPosition;
        wad.Installed += (wad) => OnInstalled(wad, position);
        _queueWadsMoney.Add(wad);
    }

    private void OnInstalled(WadMoney wad, Vector3 position)
    {
        Add(wad, position);
        _queueWadsMoney.Remove(wad);
        wad.Installed -= (wad) => OnInstalled(wad, position);
    }

    private void Add(WadMoney wadMoney, Vector3 position)
    {
        _wadsMoney.Add(wadMoney);
        wadMoney.transform.parent = transform;
        wadMoney.InstallPosition(position);
    }
}
