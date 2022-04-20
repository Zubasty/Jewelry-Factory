using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InformationPlaceFactoryGems
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Image _icon;

    public TextMeshPro Text => _text;

    public Image Icon => _icon;
}
