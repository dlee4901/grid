using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TileTerrain
{ 
    Default,
    Void
}

public class Tile : MonoBehaviour
{
    public int Id { get; set; }
    public TileTerrain? Terrain { get; set; }
    [SerializeField] SpriteRenderer _sprite;
    // [SerializeField] BoxCollider2D _collider;

    public bool Hovered { get; set; }

    public Tile()
    {
        Id = -1;
        Terrain = TileTerrain.Default;
    }

    public void OnSelect(InputAction.CallbackContext ctx) 
    {
        if (ctx.started && Hovered)
        {
            _sprite.material.color = new Color(0.2f, 0.2f, 0.2f);
        }
    }
    
    void OnMouseEnter()
    {
        Hovered = true;
        _sprite.material.color = new Color(0.5f, 0.5f, 0.5f);
        EventManager.Singleton.StartTileEvent(Id);
    }

    void OnMouseExit()
    {
        Hovered = false;
        _sprite.material.color = new Color(1.0f, 1.0f, 1.0f);
        EventManager.Singleton.StartTileEvent(-1);
    }
}
