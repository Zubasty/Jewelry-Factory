using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResources))]
public class Backpack : MonoBehaviour
{
    private PlaceForResources _placeForResources;

    public int CountMoney
    {
        get
        {
            int count = 0;

            for(int i = 0; i<_placeForResources.CountPoints; i++)
            {
                PointForResource point = _placeForResources.GetPoint(i);

                for(int j = 0; j < point.CountRecources; j++)
                {
                    if(point.GetResource(j) is WadMoney wad)
                    {
                        count += wad.ValueMoney;
                    }
                    else
                    {
                        throw new InvalidCastException("Откуда в рюкзаке что-то кроме пачек денег?");
                    }
                }
            }

            return count;
        }
    }

    public void Add(WadMoney wad) => _placeForResources.Add(wad);

    public int TakeMoney(int value)
    {
        int takedValue = 0;

        while (takedValue < value && _placeForResources.HaveResources)
        {
            if (_placeForResources.GetResource() is WadMoney wadMoney)
            {
                wadMoney.TakeMoney(value - takedValue, out int takedValueFromWad);
                takedValue += takedValueFromWad;
            }
            else
            {
                throw new InvalidCastException("Откуда в рюкзаке что-то кроме пачек денег?");
            }
        }

        return takedValue;
    }

    private void Awake()
    {
        _placeForResources = GetComponent<PlaceForResources>();
    }
}
