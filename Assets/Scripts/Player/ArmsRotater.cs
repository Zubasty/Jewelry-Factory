using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ArmsRotater : MonoBehaviour
{
    [SerializeField] private PlayerMover _mover;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _mover.StartedMove += OnStartedMove;
        _mover.StoppedMove += OnStoppedMove;
    }

    private void OnDisable()
    {
        _mover.StartedMove -= OnStartedMove;
        _mover.StoppedMove -= OnStoppedMove;
    }

    private void OnStoppedMove()
    {
        _animator.SetBool(AnimatorArms.Params.IsMove, false);
    }

    private void OnStartedMove()
    {
        _animator.SetBool(AnimatorArms.Params.IsMove, true);
    }
}
