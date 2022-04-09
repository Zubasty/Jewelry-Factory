using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransmitterAbstract : MonoBehaviour
{
    [SerializeField] private float _delay;

    private int _countCall;

    protected Action<WadMoney> ActionAfterStartInstall;

    public event Action<WadMoney> Installed;
    public event Action Transferred;

    protected abstract Vector3 ActualPosition { get; }
    protected abstract Vector3? ResultAngle { get; }

    protected float Delay => _delay;

    public void Transfer(Mover mover, Queue<WadMoney> wads)
    {
        StartCoroutine(Transfering(mover, wads, Delay));
    }

    private IEnumerator Transfering(Mover mover, Queue<WadMoney> wads, float delay)
    {
        int expectedCount = wads.Count;
        _countCall = 0;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (wads.Count > 0)
        {
            WadMoney lastWad = wads.Dequeue();

            if(ResultAngle.HasValue)
                lastWad.StartInstall(mover, ActualPosition, ResultAngle.Value);
            else
                lastWad.StartInstall(mover, ActualPosition, lastWad.transform.rotation.eulerAngles);

            ActionAfterStartInstall?.Invoke(lastWad);
            lastWad.Installed += (wad) => EndTransfer(wad, expectedCount);
            yield return wait;
        }

        yield return null;
    }

    private void EndTransfer(WadMoney wad, int expectedCount)
    {
        _countCall++;
        Installed?.Invoke(wad);
        wad.Installed -= (wad) => EndTransfer(wad, expectedCount);

        if (_countCall == expectedCount)
        {
            Transferred?.Invoke();
        }
    }
}
