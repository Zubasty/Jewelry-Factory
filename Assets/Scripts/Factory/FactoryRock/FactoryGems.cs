using UnityEngine;

public class FactoryGems : FactoryAbstractRocks<Gem>
{
    [SerializeField] private PlaceResourcesFactoryGems _placeResources;
    
    private StandFactoryGem _stand;

    protected override bool CanSpawn => _placeResources.HaveResources;
    public bool HaveResources => _placeResources.HaveResources;

    protected override void OnValidate()
    {
        if(PointForSpawn.TryGetComponent(out StandFactoryGem stand))
            _stand = stand;
        else
            Debug.LogError("Точка спавна для завода драгоценных камней может быть только стэндом");
    }

    private void OnEnable()
    {
        StartedSpawn += () => _stand.Add(_placeResources.GiveResource());
        Spawned += (gem) => _stand.GiveResource().Destroy();
    }

    private void OnDisable()
    {
        StartedSpawn -= () => _stand.Add(_placeResources.GiveResource());
        Spawned -= (gem) => _stand.GiveResource().Destroy();
    }
}
