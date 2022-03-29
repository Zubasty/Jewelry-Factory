using UnityEngine;

[RequireComponent(typeof(PlaceForResources))]
public class Arms : MonoBehaviour
{
    private PlaceForResources _place;

    public void Add(IResource resource) => _place.Add(resource);

    public IResource GiveResource() => _place.GiveResource();

    private void Awake()
    {
        _place = GetComponent<PlaceForResources>();
    }
}
