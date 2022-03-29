using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteCashRegister : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private int _amount;
    [SerializeField] private CashRegister _cashRegisterPrefab;

    public event Action<IObjectInteractive> EndedInterection;

    public void Interaction(Player player)
    {
        if(player.Money >= _amount)
        {
            BuildCashRegister();
            player.Pay(_amount);
        }

        EndedInterection?.Invoke(this);
    }

    private void BuildCashRegister()
    {
        Instantiate(_cashRegisterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
