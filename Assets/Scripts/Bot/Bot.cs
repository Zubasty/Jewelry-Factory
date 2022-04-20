using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private const int CountStates = 2;

    [SerializeField] private List<CashRegister> _cashesRegister;
    [SerializeField] private float _minTimeThink;
    [SerializeField] private float _maxTimeThink;
    [SerializeField] private float _minOffsetZ;
    [SerializeField] private float _maxOffsetZ;
    [SerializeField] private float _minOffsetX;
    [SerializeField] private float _maxOffsetX;
    [SerializeField] private float _speed;
    [SerializeField] private int _minCountBuy;
    [SerializeField] private int _maxCountBuy;
    [SerializeField] private WadMoney _template;

    public event Action StartedMove;
    public event Action StartedIdle;

    private void Start()
    {
        StartCoroutine(Living());
    }

    private void Buy(CashRegister cashRegister)
    {
        int countBuy = Mathf.Min(cashRegister.CountGems, UnityEngine.Random.Range(_minCountBuy, _maxCountBuy));
        Queue<WadMoney> wadsMoney = new Queue<WadMoney>();

        for (int i = 0; i < countBuy; i++)
        {
            Gem gem = cashRegister.GiveResource();
            List<WadMoney> wadsMoneyOneGem = new List<WadMoney>();

            while (wadsMoneyOneGem.Sum(wad => wad.Amount) < gem.Price)
            {
                wadsMoneyOneGem.Add(Instantiate(_template, transform.position, _template.transform.rotation));
                wadsMoney.Enqueue(wadsMoneyOneGem[wadsMoneyOneGem.Count - 1]);
            }

            gem.Destroy();
        }

        cashRegister.Pay(wadsMoney);
    }

    private IEnumerator Living()
    {
        while (enabled)
        {       
            int indexNextAction = UnityEngine.Random.Range(0, CountStates);

            if(indexNextAction == 0)
            {
                yield return StartCoroutine(Thinking());
            }
            else if(indexNextAction == 1)
            {
                CashRegister cashRegister = _cashesRegister[UnityEngine.Random.Range(0, _cashesRegister.Count)];

                if (cashRegister.gameObject.activeSelf)
                {
                    yield return StartCoroutine(Moving(cashRegister));
                    bool isBuying = UnityEngine.Random.Range(0, 2) == 1;

                    if (isBuying && cashRegister.CountGems > 0)
                    {
                        Buy(cashRegister);
                    }
                }

                yield return null;
            }
        }
    }

    private IEnumerator Thinking()
    {
        StartedIdle?.Invoke();
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minTimeThink, _maxTimeThink));
    }

    private IEnumerator Moving(CashRegister cash)
    {
        StartedMove?.Invoke();
        Vector3 targetPosition = new Vector3(cash.transform.position.x + UnityEngine.Random.Range(_minOffsetX, _maxOffsetX),
            transform.position.y, cash.transform.position.z + UnityEngine.Random.Range(_minOffsetZ, _maxOffsetZ));
        transform.LookAt(targetPosition);

        while(transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed*Time.deltaTime);
            yield return null;
        }

        transform.LookAt(new Vector3(cash.transform.position.x, transform.position.y, cash.transform.position.z));
        yield return null;
    }
}
