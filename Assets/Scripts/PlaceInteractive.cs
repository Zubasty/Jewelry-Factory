using UnityEngine;

public class PlaceInteractive : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _objectInteractive;
    private IObjectInteractive ObjectInteractive => (IObjectInteractive)_objectInteractive;

    private void OnValidate()
    {
        if (_objectInteractive is IObjectInteractive)
            return;

        Debug.LogError(_objectInteractive.name + " needs to implement " + nameof(IObjectInteractive));
        _objectInteractive = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player) && player.IsBusy == false)
        {
            if (ObjectInteractive.CanInterection(player)) 
                player.StopForInteraction(ObjectInteractive);
        }
    }
}
