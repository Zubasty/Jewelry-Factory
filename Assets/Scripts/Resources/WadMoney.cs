using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WadMoney : MonoBehaviour
{
    public event Action<WadMoney> Installed;

    public void InstallPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    public void StartInstall(Vector3 targetPosition, Vector3 angle, float speed)
    {
        StartCoroutine(Installing(targetPosition, angle, speed));
    }

    private IEnumerator Installing(Vector3 targetPosition, Vector3 angle, float speed)
    {
        Vector3 startPosition = transform.position;
        Vector3 startAngle = transform.rotation.eulerAngles;

        while (transform.position != targetPosition)
        {
            InstallPosition(Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed));
            float percent = 1 - Vector3.Distance(transform.position, targetPosition)/
                Vector3.Distance(startPosition, targetPosition);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startAngle, angle, percent));
            yield return null;
        }

        Installed?.Invoke(this);
        yield return null;
    }
}