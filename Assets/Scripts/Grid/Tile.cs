using UnityEngine;
using UnityEngine.InputSystem;

public enum TileTerrain
{ 
    Default,
    Void
}

public class Tile : MonoBehaviour
{
    public int Id {get; set;}

    //TileTerrain _terrain;
    bool _hovered;
    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        Id = -1;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingLayerName = "Grid";
    }

    public void OnSelect(InputAction.CallbackContext ctx) 
    {
        if (ctx.started && _hovered)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
        }
    }
    
    void OnMouseEnter()
    {
        _hovered = true;
        _spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f);
        EventManager.Singleton.StartTileHoverEvent(Id);
    }

    void OnMouseExit()
    {
        _hovered = false;
        _spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f);
        EventManager.Singleton.StartTileHoverEvent(-1);
    }
}
