using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _powerCurve;
    [SerializeField] private float _speed;

    public event Action<Mover> Moved;

    public void StartMove(WadMoney wad, Vector3 targetPosition, Vector3 angle)
    {
        StartCoroutine(Moving(wad, targetPosition, angle, _speed));
    }

    private IEnumerator Moving(WadMoney wad, Vector3 targetPosition, Vector3 angle, float speed)
    {
        Vector3 startPosition = wad.transform.position;
        Vector3 startAngle = wad.transform.rotation.eulerAngles;
        Vector3 expectedPosition = startPosition;

        while (wad.transform.position != targetPosition)
        {
            expectedPosition = Vector3.MoveTowards(expectedPosition, targetPosition, Time.deltaTime * speed);
            float percent = 1 - Vector3.Distance(expectedPosition, targetPosition) /
                Vector3.Distance(startPosition, targetPosition);
            Vector3 position = expectedPosition;
            position.y += _curve.Evaluate(percent) * _powerCurve;
            wad.InstallPosition(position);
            wad.transform.rotation = Quaternion.Euler(Vector3.Lerp(startAngle, angle, percent));
            yield return null;
        }

        wad.Install(targetPosition, angle);
        yield return null;
    }
}
