using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(PlaceForGems))]
public class CashRegister : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private int _maxCountResourcesForPoint;

    private PlaceForGems _place;

    public event Action<IObjectInteractive> EndedInterection;

    public void Interaction(Player player)
    {
        StartCoroutine(Installing(player));
    }

    private void Awake()
    {
        _place = GetComponent<PlaceForGems>();
    }

    private IEnumerator Installing(Player player)
    {       
        while (player.HaveResourcesInArms && _place.CountResources < _place.CountPoints * _maxCountResourcesForPoint)
        {
            if(player.GetFromArms() is Gem gem)
            {
                _place.Add((Gem)player.ThrowFromArms());          
            }
            else
            {
                break;
            }
            
            yield return null;
        }

        EndedInterection?.Invoke(this);
        yield return null;
    }
}
