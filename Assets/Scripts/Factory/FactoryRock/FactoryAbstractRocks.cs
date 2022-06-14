using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FactoryAbstractRocks<T> : MonoBehaviour where T : RockAbstract
{
    [SerializeField] private List<ElementForSpawn<T>> _elements;
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _pointForSpawn;

    private float _timeBeforeSpawn;
    private Coroutine _spawnning;

    public event Action StartedSpawn;
    public event Action<T> Spawned;
    public event Action<float> Filling;
    public event Action Idling;

    public Transform PointForSpawn => _pointForSpawn;

    public IReadOnlyList<int> PercentsSpawn
    {
        get
        {
            List<int> list = new List<int>();
            float sumWeight = _elements.Sum(element => element.Weight);

            foreach (var element in _elements)
                list.Add(Mathf.RoundToInt(element.Weight * 100 / sumWeight));

            return list;
        }
    }

    protected abstract bool CanSpawn { get; }

    protected virtual void OnValidate()
    {
        if (_cooldown < 0)
            _cooldown = 0;
    }

    private void Start()
    {
        _timeBeforeSpawn = _cooldown;

        foreach(var element in _elements)
        {
            for(int i = 0; i<element.StartCount; i++)
            {
                T rock = Instantiate(element.Template, _pointForSpawn.position, element.Template.transform.rotation);
                element.Place.Install(rock);
            }
        }
    }

    private void Update()
    {
        if(CanSpawn && _spawnning == null)
            _spawnning = StartCoroutine(Spawnning());

        if(_spawnning == null)
            Idling?.Invoke();
    }

    private IEnumerator Spawnning()
    {
        StartedSpawn?.Invoke();

        while(_timeBeforeSpawn > 0)
        {
            _timeBeforeSpawn -= Time.deltaTime;
            Filling?.Invoke(1 - _timeBeforeSpawn / _cooldown);
            yield return null;
        }

        _timeBeforeSpawn = _cooldown;
        Spawn();
        _spawnning = null;
        yield return null;
    }

    private void Spawn()
    {
        float sumWeight = _elements.Sum(element => element.Weight);
        float accumulateWeight = 0;
        float chance = UnityEngine.Random.value;

        for(int i = 0; i < _elements.Count; i++)
        {
            accumulateWeight += _elements[i].Weight;
            
            if(accumulateWeight / sumWeight >= chance)
            {
                T rock = Instantiate(_elements[i].Template, _pointForSpawn.position, _elements[i].Template.transform.rotation);
                _elements[i].Place.Add(rock);
                Spawned?.Invoke(rock);
                break;
            }
        }
    }
}
