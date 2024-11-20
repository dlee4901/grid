using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public Image image;

    public void Init(string name, Sprite sprite, Transform parent)
    {
        image = gameObject.AddComponent<Image>();
        gameObject.name = name;
        image.sprite = sprite;
        gameObject.transform.SetParent(parent);
    }
}