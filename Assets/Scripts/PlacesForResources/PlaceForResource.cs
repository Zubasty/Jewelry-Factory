using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceForResource : MonoBehaviour
{
    [SerializeField] private float _offsetY;
    [SerializeField] private List<PointForResource> _points;

    public int CountResources => _points.Sum(point => point.CountResources);

    public int CountPoints => _points.Count;

    public Vector3 ActualPosition => PointForAdd.ActualPosition;

    public PointForResource FirstPoint => _points[0];

    public IReadOnlyList<IResource> Resources
    {
        get
        {
            List<IResource> resources = new List<IResource>();

            for(int i = 0; i < _points.Count; i++)
            {
                for(int j = 0; j < _points[i].Resources.Count; j++)
                    resources.Add(_points[i].Resources[j]);
            }

            return resources;
        }
    }

    private PointForResource PointForAddWithoutQueue
    {
        get
        {
            for (int i = 0; i < _points.Count - 1; i++)
            {
                if (_points[i + 1].CountResources < _points[i].CountResources)
                    return _points[i + 1];
            }

            return _points[0];
        }
    }

    private PointForResource PointForAdd
    {
        get
        {
            for(int i = 0; i < _points.Count - 1; i++)
            {
                if(_points[i+1].CountResourcesWithQueue < _points[i].CountResourcesWithQueue)
                    return _points[i+1];
            }

            return _points[0];
        }
    }

    private PointForResource PointForGive
    {
        get
        {
            for (int i = 0; i < _points.Count - 1; i++)
            {
                if (_points[i + 1].CountResources < _points[i].CountResources)
                    return _points[i];
            }

            return _points[_points.Count - 1];
        }
    }

    private void Awake()
    {
        foreach (var point in _points)
            point.Init(_offsetY);
    }

    public Queue<IResource> GiveResources(int count)
    {
        Queue<IResource> givenWads = new Queue<IResource>();

        while (givenWads.Count < count)
            givenWads.Enqueue(GiveResource());

        return givenWads;
    }
    
    public Queue<IResource> GiveResources() => GiveResources(CountResources);

    public void Install(IResource resource) => PointForAdd.Add(resource);

    public void StartAdd(IResource resource) => PointForAdd.AddInQueue(resource);

    private IResource GiveResource() => PointForGive.GiveResource();
}
