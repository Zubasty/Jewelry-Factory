using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MoneyImage : MonoBehaviour
{
    private Image _image;
    private float _speed;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Init(Vector3 position, float speed)
    {
        _image.rectTransform.localPosition = position;
        _speed = speed;
    }

    public void GoToEndPath(Image image)
    {
        StartCoroutine(Ending(image));
    }

    private IEnumerator Ending(Image targetImage)
    {
        while(targetImage.rectTransform.position != _image.rectTransform.position)
        {
            _image.rectTransform.position = Vector3.MoveTowards(_image.rectTransform.position, 
                targetImage.rectTransform.position, _speed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}
