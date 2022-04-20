using UnityEngine;

public class Gem : RockAbstract
{
    [SerializeField] private int _price;

    public int Price => _price;
}
