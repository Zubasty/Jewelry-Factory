using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Factory<T> : MonoBehaviour where T : RockAbstract
{
    [SerializeField] private List<RockPrefabForFactory<T>> _templates = new List<RockPrefabForFactory<T>>();
    [SerializeField] private float _timeProduction;
    [SerializeField] private PlaceForAbstractRock<T> _place;

    private float[] _chancesCreate;
    private float _timeBeforeProduction;

    protected virtual bool CanCreate { get => true; }

    private void Awake()
    {
        _chancesCreate = new float[_templates.Count];
        int sumWeight = _templates.Sum(template => template.WeightSpawn);
        int accumulatedSum = 0;

        for(int i = 0; i < _templates.Count; i++)
        {
            accumulatedSum += _templates[i].WeightSpawn;
            _chancesCreate[i] = (float)accumulatedSum / sumWeight;
        }

        _timeBeforeProduction = _timeProduction;
    }

    private void Update()
    {
        _timeBeforeProduction = Mathf.Max(_timeBeforeProduction - Time.deltaTime, 0);
      
        if (_timeBeforeProduction == 0 && CanCreate)
        {
            CreateRock();
            _timeBeforeProduction = _timeProduction;
        }
    }

    private void CreateRock()
    {
        float chance = Random.value;

        for(int i = 0; i < _chancesCreate.Length-1; i++)
        {
            if (chance < _chancesCreate[i + 1])
            {
                _place.Add(Instantiate(_templates[i].Prefab));
                return;
            }
        }
        _place.Add(Instantiate(_templates[_templates.Count - 1].Prefab));
    }
}
