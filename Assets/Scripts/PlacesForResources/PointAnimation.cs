using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointForResource))]
public class PointAnimation : MonoBehaviour
{
    [SerializeField] private bool _takeAxisX;
    [SerializeField] private bool _takeAxisY;
    [SerializeField] private bool _takeAxisZ;
    [SerializeField] private float _speed;

    private PointForResource _point;

    private void Awake()
    {
        _point = GetComponent<PointForResource>();
    }

    private void Update()
    {
        foreach (IResource resource in _point.Resources)
            resource.Rotate(_takeAxisX ? _speed * Time.deltaTime : 0, _takeAxisY ? _speed * Time.deltaTime : 0, _takeAxisZ ? _speed * Time.deltaTime : 0);
    }
}
