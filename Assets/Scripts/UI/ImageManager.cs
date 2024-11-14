using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    Image _image;
    EventHandler _eventHandler;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(string name, Sprite sprite, Transform parent)
    {
        _image = gameObject.AddComponent<Image>();
        _eventHandler = gameObject.AddComponent<EventHandler>();
        gameObject.name = name;
        _image.sprite = sprite;
        gameObject.transform.SetParent(parent);
    }

    public void Init(string name, Sprite sprite, Transform parent, int id)
    {
        Init(name, sprite, parent);
        _eventHandler.id = id;
    }
}