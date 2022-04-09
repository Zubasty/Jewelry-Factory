using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WadMoney : MonoBehaviour
{
    [SerializeField] private int _amount;

    public event Action<WadMoney> Installed;

    public int Amount => _amount;

    public void InstallPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    public void Install(Vector3 position, Vector3 angle)
    {
        InstallPosition(position);
        transform.rotation = Quaternion.Euler(angle);
        Installed?.Invoke(this);
    }

    public void StartInstall(Mover mover, Vector3 targetPosition, Vector3 angle)
    {
        mover.StartMove(this, targetPosition, angle);
    }
}