using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeAnimation : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _minSize;

    public event Action EndedAnimation;

    private void OnValidate()
    {
        if(_minSize > _maxSize)
        {
            _minSize = _maxSize;
        }
    }

    public void StartAnimation(Transform transform)
    {
        StartCoroutine(Animating(transform));
    }

    private IEnumerator Animating(Transform transform)
    {
        Vector3 resultScale = transform.localScale;
        Vector3 maxScale = transform.localScale * _maxSize;
        transform.localScale = resultScale * _minSize;

        yield return StartCoroutine(SettingSize(transform, maxScale));
        yield return StartCoroutine(SettingSize(transform, resultScale));
        EndedAnimation?.Invoke();
    }

    private IEnumerator SettingSize(Transform transform, Vector3 resultScale)
    {
        while (transform.localScale != resultScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, resultScale, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
