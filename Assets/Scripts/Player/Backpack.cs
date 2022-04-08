using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceForWadsMoney))]
public class Backpack : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private Vector3 _resultAngle;
    [SerializeField] private float _speedInstalling;

    private PlaceForWadsMoney _place;
    private int _countCall = 0; // попахивает нарушением зоны ответственности

    public event Action TakedWadsMoney;

    public void TakeWadsMoney(ListWadsMoney wads)
    {
        StartCoroutine(Folding(wads, new Vector3(_resultAngle.x, _resultAngle.y, 
            _resultAngle.z - transform.rotation.eulerAngles.y), _speedInstalling));
    }

    private IEnumerator Folding(ListWadsMoney wads, Vector3 angle, float speed)
    {
        int expectedCount = wads.CountWads;
        _countCall = 0;
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while (wads.CountWads > 0)
        {
            WadMoney lastWad = wads.GiveWadMoney();
            _place.StartAdd(lastWad, angle, speed);
            lastWad.Installed += (wad) => OnInstalled(wad, expectedCount);
            yield return wait;
        }

        yield return null;
    }

    private void OnInstalled(WadMoney wad, int expectedCount)
    {
        _countCall++;

        if (_countCall == expectedCount)
        {
            TakedWadsMoney?.Invoke();
        }

        wad.Installed -= (wad) => OnInstalled(wad, expectedCount);
    }

    private void Awake()
    {
        _place = GetComponent<PlaceForWadsMoney>();
    }
}
