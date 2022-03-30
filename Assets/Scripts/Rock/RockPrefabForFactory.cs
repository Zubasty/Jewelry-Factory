using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RockPrefabForFactory<T> where T : RockAbstract
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _weightSpawn;

    public int WeightSpawn => _weightSpawn;

    public T Prefab => _prefab;
}
