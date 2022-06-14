using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Arms))]
public class ArmsAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private float _speed;
    [SerializeField] private float _amplitude;

    private Arms _arms;
    private Quaternion _defaultAngle;
    private bool _isPlaying;
    private float _direction;

    private void Awake()
    {
        _arms = GetComponent<Arms>();
    }

    private void OnEnable()
    {
        _arms.StartedTakeResources += OnStartedTakeResources;
        _arms.TakedResources += OnTakedResources;
    }

    private void Start()
    {
        _defaultAngle = transform.localRotation;
        _isPlaying = false;
        _direction = 1;
    }

    private void Update()
    {
        if (_isPlaying)
        {
            if (_mover.IsMove)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Time.deltaTime * _speed * _direction) + transform.localEulerAngles);

                if (Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.z, _defaultAngle.eulerAngles.z)) > _amplitude)
                {
                    transform.localRotation = Quaternion.Euler(_defaultAngle.eulerAngles + new Vector3(0, 0, _amplitude * _direction));
                    _direction *= -1;
                }
            }
            else
            {          
                transform.localRotation = Quaternion.Euler(new Vector3(_defaultAngle.eulerAngles.x, _defaultAngle.eulerAngles.y,
                    Mathf.MoveTowardsAngle(transform.localEulerAngles.z, _defaultAngle.eulerAngles.z, _speed * Time.deltaTime)));
            }

        }
    }

    private void OnDisable()
    {
        _arms.StartedTakeResources -= OnStartedTakeResources;
    }

    private void OnStartedTakeResources()
    {
        _isPlaying = false;
        transform.localRotation = _defaultAngle;
    }

    private void OnTakedResources()
    {
        _isPlaying = true;
    }
}
