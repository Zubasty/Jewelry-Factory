using UnityEngine;

public class WadMoney : ResourceAbstract
{
    [SerializeField] private int _amount;

    public int Amount => _amount;
}