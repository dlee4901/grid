using UnityEngine;
using UnityHFSM;

public enum TileTerrain { Default, Void }
public enum TileState { Default, Hovered, Selected }

public class Tile : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider;
    //TileTerrain _terrain;

    Color _colorDefault = new Color(1.0f, 1.0f, 1.0f);
    Color _colorHovered = new Color(0.5f, 0.5f, 0.5f);
    Color _colorSelected = new Color(0f, 1.0f, 1.0f);
    Color _colorUnselectable = new Color(0.2f, 0.2f, 0.2f);

    bool _selectable;
    public bool Selectable
    {
        get { return _selectable; }
        set { _selectable = value; OnPropertyChanged("Selectable"); }
    }

    TileState _state;
    public TileState State
    {
        get { return _state; }
        set { _state = value; OnPropertyChanged("State"); }
    }
    
    public int Id {get; set;}

    void Awake()
    {

    }

    void Update()
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
        Selectable = true;
    }

    void OnPropertyChanged(string property)
    {
        if (property == "Selectable")
        {
            if (_selectable)
            {
                _spriteRenderer.color = _colorDefault;
            }
            else
            {
                _spriteRenderer.color = _colorUnselectable;
            }
        }
        else if (property == "State")
        {
            if (_state == TileState.Hovered)       SetColor(_colorHovered);
            else if (!_selectable)                 SetColor(_colorUnselectable);
            else if (_state == TileState.Selected) SetColor(_colorSelected);
            else                                   SetColor(_colorDefault);
        }
    }

    void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
    
    void OnMouseEnter()
    {
        if (Selectable && State != TileState.Selected) State = TileState.Hovered;
        EventManager.Singleton.StartTileHoverEvent(Id);
    }

    void OnMouseExit()
    {
        if (Selectable && State != TileState.Selected) State = TileState.Default;
        EventManager.Singleton.StartTileHoverEvent(0);
    }
}
