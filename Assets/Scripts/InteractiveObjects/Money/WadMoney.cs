using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WadMoney : MonoBehaviour, IResource
{
    [SerializeField] private int _valueMoney;

    public event Action<IResource> Used;

    public int ValueMoney
    {
        get => _valueMoney;
        private set 
        { 
            if(value < 0)
            {
                throw new ArgumentOutOfRangeException("В пачке не может оказаться отрицательное количество денег");
            }
            
            _valueMoney = value;

            if(_valueMoney == 0)
            {
                Used?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }

    public void TakeMoney(int value, out int takedValue)
    {
        if(value < 0)
        {
            throw new ArgumentOutOfRangeException("Ты не можешь взять из пачки отрицательное количество денег");
        }

        if(value > ValueMoney)
        {
            takedValue = ValueMoney;
            ValueMoney = 0;
        }
        else
        {
            takedValue = value;
            ValueMoney -= value;
        }
    }

    public void Install(Vector3 position, Transform parent)
    {
        transform.position = position;
        transform.parent = parent;
    }
}
