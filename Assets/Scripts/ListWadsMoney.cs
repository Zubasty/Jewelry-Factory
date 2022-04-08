using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForWadsMoney))]
public class ListWadsMoney : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private WadMoney _template;
    [SerializeField] private int _count;

    private PlaceForWadsMoney _place;

    public event Action<IObjectInteractive> EndedInterection;

    public int CountWads => _place.CountWads;

    public WadMoney GiveWadMoney() => _place.GiveWadMoney();

    public void Interection(Player player)
    {
        player.TakeWadsMoney(this);
        player.TakedWadsMoney += OnTakedWadsMoney;
    }

    private void OnTakedWadsMoney(Player player)
    {
        player.TakedWadsMoney -= OnTakedWadsMoney;
        EndedInterection?.Invoke(this);
        Destroy(gameObject);
    }

    private void Awake()
    {
        _place = GetComponent<PlaceForWadsMoney>();
    }

    private void Start()
    {
        for (int i = 0; i < _count; i++)
        {
            _place.Install(Instantiate(_template));
        }
    }
}
