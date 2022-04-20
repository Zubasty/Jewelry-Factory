using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyVisual : MonoBehaviour
{
    [SerializeField] private PlayerMoney _playerMoney;
    [SerializeField] private MoneyImage _moneyImage;
    [SerializeField] private Image _targetImage;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _sprayX;
    [SerializeField] private float _sprayY;
    [SerializeField] private float _speed;

    private List<MoneyImage> _images;

    private void Awake()
    {
        _playerMoney = GetComponent<PlayerMoney>();
        _images = new List<MoneyImage>();
    }

    private void OnEnable()
    {
        _playerMoney.TakedWadMoney += OnTakedWadMoney;
        _playerMoney.TakedAllMoney += OnTakedAllMoney;
        _playerMoney.SetedMoney += (money) => _text.text = money.ToString();
    }

    private void Start()
    {
        _text.text = _playerMoney.Money.ToString();
    }

    private void OnDisable()
    {
        _playerMoney.TakedWadMoney -= OnTakedWadMoney;
        _playerMoney.TakedAllMoney -= OnTakedAllMoney;
        _playerMoney.SetedMoney -= (money) => _text.text = money.ToString();
    }

    private void OnTakedWadMoney()
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range(-_sprayX, _sprayX), UnityEngine.Random.Range(-_sprayY, _sprayY), 0);
        MoneyImage moneyImage = Instantiate(_moneyImage, _canvas.transform);
        moneyImage.Init(position, _speed);
        _images.Add(moneyImage);
    }

    private void OnTakedAllMoney()
    {        
        foreach(MoneyImage image in _images)
            image.GoToEndPath(_targetImage);

        _images.Clear();
    }
}
