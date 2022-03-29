using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlaceForResources))]
public class ListWadMoney : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private float _countWads;
    [SerializeField] private WadMoney _wadPrefab;

    private PlaceForResources _place;
    private List<WadMoney> _wadsMoney;

    public event Action<IObjectInteractive> EndedInterection;

    public void Interaction(Player player)
    {
        StartCoroutine(Installing(player));   
    }

    private void Awake()
    {
        _place = GetComponent<PlaceForResources>();
        _wadsMoney = new List<WadMoney>();
    }

    private void Start()
    {
        for(int i = 0; i < _countWads; i++)
        {
            _wadsMoney.Add(Instantiate(_wadPrefab));
            _place.Add(_wadsMoney[_wadsMoney.Count - 1]);
        }
    }

    private IEnumerator Installing(Player player)
    {
        for(int i = 0; i < _wadsMoney.Count; i++)
        {
            player.TakeInBackpack(_wadsMoney[i]);
            yield return null;
        }
        EndedInterection?.Invoke(this);
        Destroy(gameObject);
        yield return null;
    }
}
