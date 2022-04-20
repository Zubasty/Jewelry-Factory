using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _powerCurve;
    [SerializeField] private float _speed;

    public void StartMove(IResource resource, Vector3 targetPosition, Vector3 angle) =>
        StartCoroutine(Moving(resource, targetPosition, angle, _speed));

    private IEnumerator Moving(IResource resource, Vector3 targetPosition, Vector3 angle, float speed)
    {
        Vector3 startPosition = resource.Position;
        Vector3 startAngle = resource.Angle;
        Vector3 expectedPosition = startPosition;

        while (resource.Position != targetPosition)
        {
            expectedPosition = Vector3.MoveTowards(expectedPosition, targetPosition, Time.deltaTime * speed);
            float percent = 1 - Vector3.Distance(expectedPosition, targetPosition) /
                Vector3.Distance(startPosition, targetPosition);
            Vector3 position = expectedPosition;
            position.y += _curve.Evaluate(percent) * _powerCurve;
            resource.Move(position, Vector3.Lerp(startAngle, angle, percent));
            yield return null;
        }

        resource.Install(targetPosition, angle);
        yield return null;
    }
}
