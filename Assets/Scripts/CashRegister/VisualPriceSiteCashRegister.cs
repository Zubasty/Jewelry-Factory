using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class VisualPriceSiteCashRegister : MonoBehaviour
{
    [SerializeField] private SiteCashRegister _site;

    private TextMeshPro _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        _site.SetedPrice += (price) => _text.text = price.ToString();
    }

    private void OnDisable()
    {
        _site.SetedPrice -= (price) => _text.text = price.ToString();
    }

    private void Start()
    {
        _text.text = _site.Price.ToString();
    }
}
