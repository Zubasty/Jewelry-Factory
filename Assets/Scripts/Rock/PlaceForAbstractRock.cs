using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForResources))]
public abstract class PlaceForAbstractRock<T> : MonoBehaviour, IObjectInteractive where T : RockAbstract
{
    private PlaceForResources _resources;

    public event Action<IObjectInteractive> EndedInterection;

    public bool HaveResources => _resources.HaveResources;

    public int CountResources => _resources.CountResources;

    public int CountPoints => _resources.CountPoints;

    private void Awake()
    {
        _resources = GetComponent<PlaceForResources>();
    }

    public void Add(T rock)
    {
        _resources.Add(rock);
    }

    public T GiveRock()
    {
        if (_resources.GiveResource() is T rock)
        {
            return rock;
        }

        throw new InvalidCastException($"В этом месте для камней могут находиться только камни типа {nameof(T)}");
    }

    public void Interaction(Player player)
    {
        StartCoroutine(Installing(player));
    }

    private IEnumerator Installing(Player player)
    {
        while (_resources.HaveResources)
        {
            player.TakeInArms(GiveRock());
            yield return null;
        }

        EndedInterection?.Invoke(this);
        yield return null;
    }
}
