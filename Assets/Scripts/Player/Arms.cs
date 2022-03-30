using UnityEngine;
using System;

[RequireComponent(typeof(PlaceForResources))]
public class Arms : MonoBehaviour
{
    private PlaceForResources _place;

    public bool HaveResources => _place.HaveResources;

    public void Add(RockAbstract rock) 
    {
        _place.Add(rock);
    }

    public RockAbstract GetRock() => ReturnRock(_place.GetResource());

    public RockAbstract GiveRock() => ReturnRock(_place.GiveResource());

    private RockAbstract ReturnRock(IResource resource)
    {
        if (resource is RockAbstract rock)
        {
            return rock;
        }

        throw new InvalidCastException("Откуда в руках что-то кроме камней?");
    }

    private void Awake()
    {
        _place = GetComponent<PlaceForResources>();
    }
}
