using UnityEngine;
using UnityHFSM;

public enum TileTerrain { Default, Void }
public enum TileState { Default, Hovered, Selected }

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    //TileTerrain _terrain;

    private Color _colorDefault = new Color(1.0f, 1.0f, 1.0f);
    private Color _colorHovered = new Color(0.7f, 0.7f, 0.7f);
    private Color _colorUnselectable = new Color(0.4f, 0.4f, 0.4f);
    private Color _colorSelected = new Color(1.0f, 1.0f, 0f);
    private Color _colorPlayer1 = new Color(1.0f, 0.7f, 0.7f);
    private Color _colorPlayer2 = new Color(0.7f, 0.7f, 1.0f);

    public int Id { get; private set; }

    bool _selectable;
    public bool Selectable
    {
        get { return _selectable; }
        set { _selectable = value; OnPropertyChanged("Selectable"); }
    }
    int _team;
    public int Team
    {
        get { return _team; }
        set { _team = value; OnPropertyChanged("Team"); }
    }

    TileState _state;
    public TileState State
    {
        get { return _state; }
        set { _state = value; OnPropertyChanged("State"); }
    }
    
    public void Init(Sprite sprite, float tileScale, int positionIdx=0)
    {
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        _spriteRenderer.sortingLayerName = "Grid";
        _spriteRenderer.sprite = sprite;
        _boxCollider.size = new Vector2(10.24f, 10.24f);
        
        name = "Tile " + positionIdx;
        transform.localScale = new Vector3(tileScale, tileScale, transform.localScale.z);
        Id = positionIdx;
        Selectable = true;
        Team = 0;
        State = TileState.Default;
    }

    void OnPropertyChanged(string property)
    {
        if (property == "Selectable")
        {
            if (_state != TileState.Selected)
            {
                if (_selectable) SetColor(_colorDefault);
                else             SetColor(_colorUnselectable);
            }
        }
        else if (property == "Team")
        {
            if (_selectable)
            {
                SetTeamColor(_colorDefault);
            }
        }
        else if (property == "State")
        {
            if (_state == TileState.Hovered)       SetTeamColor(_colorHovered);
            else if (!_selectable)                 SetColor(_colorUnselectable);
            else if (_state == TileState.Selected) SetColor(_colorSelected);
            else                                   SetTeamColor(_colorDefault);
        }
    }

    void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    void SetTeamColor(Color color)
    {
        if (_team == 1)      SetColor(_colorPlayer1);
        else if (_team == 2) SetColor(_colorPlayer2);
        else                 SetColor(color);
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
