using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointForWadsMoney : MonoBehaviour
{
    private List<WadMoney> _wadsMoney;
    private List<WadMoney> _queueWadsMoney;
    private float _offsetY;

    private Vector3 ActualPosition => new Vector3(transform.position.x,
            transform.position.y + _offsetY * CountWads, transform.position.z);

    public int CountWads => _wadsMoney.Count + _queueWadsMoney.Count;

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

    public void StartInstall(WadMoney wadMoney, Vector3 angle, float speed)
    {
        wadMoney.StartInstall(ActualPosition, angle, speed);
        wadMoney.Installed += (wad) => EndInstall(wad);
        _queueWadsMoney.Add(wadMoney);
    }

    private void Add(WadMoney wadMoney, Vector3 position)
    {
        wadMoney.transform.parent = transform;
        wadMoney.InstallPosition(position);
        _wadsMoney.Add(wadMoney);
    }

    private void EndInstall(WadMoney wadMoney)
    {
        wadMoney.transform.parent = transform;
        _queueWadsMoney.Remove(wadMoney);
        _wadsMoney.Add(wadMoney);
        wadMoney.Installed -= (wad) => EndInstall(wad);
    }
}
