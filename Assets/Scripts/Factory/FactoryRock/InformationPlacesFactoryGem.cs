using System.Collections.Generic;
using UnityEngine;

public class InformationPlacesFactoryGem : MonoBehaviour
{
    [SerializeField] private FactoryAbstractRocks<Gem> _factory;
    [SerializeField] private List<InformationPlaceFactoryGems> _information;

    private void OnValidate()
    {
        if(_factory.PercentsSpawn.Count != _information.Count)
            Debug.LogWarning("Количество групп информации не совпадает с количеством типов производимых ресурсов");
    }

    private void Start()
    {
        int count = Mathf.Min(_factory.PercentsSpawn.Count, _information.Count);

        for(int i = 0; i < count; i++)
            _information[i].Text.text = $"{_factory.PercentsSpawn[i]} %";
    }
}
