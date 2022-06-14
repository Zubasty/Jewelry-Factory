using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mover : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _powerCurve;
    [SerializeField] private float _speed;

    public event Action<IResource> InstalledInPosition;

    public void StartMove(IResource resource, List<Vector3> targetPositions, Vector3 angle) =>
        StartCoroutine(Moving(resource, targetPositions, angle, _speed));

    private IEnumerator Moving(IResource resource, List<Vector3> targetPositions, Vector3 angle, float speed)
    {
        int numberPosition = 0;
        Vector3 startPosition = resource.Position;
        Vector3 startAngle = resource.Angle;
        Vector3 expectedPosition = startPosition;

        while(numberPosition < targetPositions.Count)
        {
            while (resource.Position != targetPositions[numberPosition])
            {
                expectedPosition = Vector3.MoveTowards(expectedPosition, targetPositions[numberPosition], Time.deltaTime * speed);
                float percent = 1 - Vector3.Distance(expectedPosition, targetPositions[numberPosition]) /
                    Vector3.Distance(startPosition, targetPositions[targetPositions.Count-1]);
                Vector3 position = expectedPosition;
                position.y += _curve.Evaluate(percent) * _powerCurve;
                resource.Move(position, Vector3.Lerp(startAngle, angle, percent));
                yield return null;
            }
            numberPosition++;
            InstalledInPosition?.Invoke(resource);
        }      

        resource.Install(targetPositions[targetPositions.Count - 1], angle);
        yield return null;
    }
}
