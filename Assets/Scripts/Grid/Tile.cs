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

    Color colorHovered = new Color(0.5f, 0.5f, 0.5f);
    Color colorAvailable = new Color(1.0f, 1.0f, 1.0f);
    Color colorUnavailable = new Color(0.2f, 0.2f, 0.2f);
    bool _hovered;
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
                _spriteRenderer.color = colorAvailable;
            }
            else
            {
                _spriteRenderer.color = colorUnavailable;
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
        _hovered = true;
        if (Available)
        {
            _spriteRenderer.color = colorHovered;
        }
        EventManager.Singleton.StartTileHoverEvent(Id);
    }

    void OnMouseExit()
    {
        _hovered = false;
        if (Available)
        {
            _spriteRenderer.color = colorAvailable;
        }
        EventManager.Singleton.StartTileHoverEvent(-1);
    }
}
