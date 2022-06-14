using System;
using UnityEngine;

[RequireComponent(typeof(PlaceForResource))]
public class ListWadsMoney : MonoBehaviour, IObjectInteractive
{
    [SerializeField] private WadMoney _template;
    [SerializeField] private int _count;

    private PlaceForResource _place;

    public event Action<IObjectInteractive> EndedInterection;

    public PlaceForResource Place => _place;

    private void Awake()
    {
        _place = GetComponent<PlaceForResource>();
    }

    private void Start()
    {
        for (int i = 0; i < _count; i++)
        {
            _place.Install(Instantiate(_template));
        }
    }

    public void Interection(Player player)
    {
        player.TakeWadsMoney(this);
        player.TakedWadsMoney += OnTakedWadsMoney;
    }

    public bool CanInterection(Player player) => true;

    private void OnTakedWadsMoney(Player player)
    {
        player.TakedWadsMoney -= OnTakedWadsMoney;
        EndedInterection?.Invoke(this);
        Destroy(gameObject);
    }
}
