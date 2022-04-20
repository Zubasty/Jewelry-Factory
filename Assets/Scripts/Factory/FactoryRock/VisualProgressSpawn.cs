using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class VisualProgressSpawn : MonoBehaviour
{
    [SerializeField] private FactoryGems _factory;
    [SerializeField] private Image _childImage;

    private CanvasGroup _group;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _factory.Filling += (value) => _childImage.fillAmount = value;
        _factory.Idling += () => _group.alpha = 0;
        _factory.StartedSpawn += () => _group.alpha = 1;
    }

    private void OnDisable()
    {
        _factory.Filling -= (value) => _childImage.fillAmount = value;
        _factory.Idling -= () => _group.alpha = 0;
        _factory.StartedSpawn -= () => _group.alpha = 1;
    }
}
