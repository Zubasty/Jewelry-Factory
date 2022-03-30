using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceForResources : MonoBehaviour
{
    [SerializeField] private List<PointForResource> _pointsForResource;
    [SerializeField] private float _offsetY;

    public bool HaveResources
    {
        get
        {
            foreach(var point in _pointsForResource)
            {
                if (point.CountRecources > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public int CountPoints => _pointsForResource.Count;

    public int CountResources => _pointsForResource.Sum(point => point.CountRecources);

    public PointForResource GetPoint(int index) => _pointsForResource[index];

    public IResource GetResource() => _pointsForResource[GetIndexForGet()].GetResource();

    public IResource GiveResource() => _pointsForResource[GetIndexForGet()].GiveResource();

    public void Add(IResource resource)
    {
        _pointsForResource[GetIndexForAdd()].Add(resource);
    }

    private int GetIndexForGet()
    {
        for (int i = 0; i < _pointsForResource.Count - 1; i++)
        {
            if (_pointsForResource[i].CountRecources > _pointsForResource[i + 1].CountRecources)
            {
                return i;
            }
        }

        return _pointsForResource.Count - 1;
    }

    private int GetIndexForAdd()
    {
        for(int i = 0; i < _pointsForResource.Count-1; i++)
        {
            if(_pointsForResource[i].CountRecources > _pointsForResource[i+1].CountRecources)
            {
                return i+1;
            }
        }

        return 0;
    }

    private void Awake()
    {
        foreach(var point in _pointsForResource)
        {
            point.Init(_offsetY);
        }
    }
}
