using System;
using UnityEngine;

[Serializable]
public class ElementForSpawn<T> where T : RockAbstract
{
    [SerializeField] private int _startCount;
    [SerializeField] private T _template;
    [SerializeField] private float _weight;
    [SerializeField] private PlaceAbstractFactory<T> _place;

    public int StartCount => _startCount;
    public T Template => _template;
    public float Weight => _weight;
    public PlaceAbstractFactory<T> Place => _place;

    public void OnValidate()
    {
        if(_weight <= 0)
            _weight = 1;
        if(_startCount<0)
            _startCount = 0;
    }
}
