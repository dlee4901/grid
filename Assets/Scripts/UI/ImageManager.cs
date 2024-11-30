using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public Image Image;

    public void Init(string name, Sprite sprite, Transform parent)
    {
        Image = gameObject.AddComponent<Image>();
        gameObject.name = name;
        Image.sprite = sprite;
        gameObject.transform.SetParent(parent);
    }
}