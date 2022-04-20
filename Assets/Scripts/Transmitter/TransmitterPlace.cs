using UnityEngine;

public class TransmitterPlace : TransmitterAbstract
{
    private PlaceForResource _place;

    protected override Vector3 ActualPosition => _place.ActualPosition;

    private void OnEnable()
    {
        ActionAfterStartInstall += Add;
    }

    private void OnDisable()
    {
        ActionAfterStartInstall -= Add;
    }

    public void Init(Mover mover, PlaceForResource place)
    {
        Init(mover);
        _place = place;
    }

    private void Add(IResource resource)
    {
        _place.StartAdd(resource);
    }
}
