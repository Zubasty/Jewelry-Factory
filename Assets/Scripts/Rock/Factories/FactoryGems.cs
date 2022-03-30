using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGems : Factory<Gem>
{
    [SerializeField] private PlaceForRocks _sourceRocks;

    protected override bool CanCreate => _sourceRocks.HaveResources;
}
