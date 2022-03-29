using UnityEngine;

public class Caster : MonoBehaviour
{
    public bool TryGetTarget(Ray ray, out TargetCaster target)
    {
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out target))
            {
                target.SetPosition(hit.point);
                return true;
            }
        }
        target = null;
        return false;
    }
}
