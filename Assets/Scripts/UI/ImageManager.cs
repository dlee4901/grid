using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void Init(string name, Sprite sprite, Transform parent)
    {
        _image = gameObject.AddComponent<Image>();
        gameObject.name = name;
        _image.sprite = sprite;
        gameObject.transform.SetParent(parent);
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }
}