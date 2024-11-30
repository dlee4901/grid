using UnityEngine;

public enum TileTerrain
{ 
    Default,
    Void
}

public class Tile : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider;
    //TileTerrain _terrain;

    Color _colorHovered = new Color(0.5f, 0.5f, 0.5f);
    Color _colorAvailable = new Color(1.0f, 1.0f, 1.0f);
    Color _colorUnavailable = new Color(0.2f, 0.2f, 0.2f);
    bool _available;
    public bool Available
    {
        get { return _available; }
        set { _available = value; OnPropertyChanged("Available"); }
    }
    public int Id {get; set;}

    void Awake()
    {

    }

    public void Init(Sprite sprite, float tileScale, int positionIdx=0)
    {
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        _spriteRenderer.sortingLayerName = "Grid";
        _spriteRenderer.sprite = sprite;
        _boxCollider.size = new Vector2(10.24f*tileScale, 10.24f*tileScale);
        
        name = "Tile " + positionIdx;
        transform.localScale = new Vector3(tileScale, tileScale, transform.localScale.z);
        Id = positionIdx;
        Available = true;
    }

    void OnPropertyChanged(string property)
    {
        if (property == "Available")
        {
            if (_available)
            {
                _spriteRenderer.color = _colorAvailable;
            }
            else
            {
                _spriteRenderer.color = _colorUnavailable;
            }
        }
    }

    // public void OnSelect(InputAction.CallbackContext ctx) 
    // {
    //     if (ctx.started && _hovered && Available)
    //     {
    //         Debug.Log("Tile OnSelect");
    //     }
    // }
    
    void OnMouseEnter()
    {
        if (Available)
        {
            _spriteRenderer.color = _colorHovered;
        }
        EventManager.Singleton.StartTileHoverEvent(Id);
    }

    void OnMouseExit()
    {
        if (Available)
        {
            _spriteRenderer.color = _colorAvailable;
        }
        EventManager.Singleton.StartTileHoverEvent(0);
    }
}
