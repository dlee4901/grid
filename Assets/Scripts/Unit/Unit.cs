using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct UnitStats
{
    public int GridPosition;
    public int PlayerController;
    public int Health;
    public int Shield;
    public DirectionFacing DirectionFacing;
}

public enum UnitAction {Move, Skill}

public class Unit : MonoBehaviour
{
    public UnitProperties Properties;
    public List<UnitMovement> Movement;
    public UnitStats Stats;

    public int ListUIPosition;
    //public bool IsDragging;

    SpriteRenderer _spriteRenderer;

    public void Init()
    {
        name = Properties.Name;
        gameObject.SetActive(false);
        transform.localScale = new Vector3(10f, 10f, 0f);

        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Properties.Sprite;
        _spriteRenderer.sortingLayerName = "Unit";
    }

    public void SetPosition(int idx, Vector3Int vec, float tileScale)
    {
        Stats.GridPosition = idx;
        transform.position = Util.Get2DWorldPos(vec, tileScale);
    }

    public bool SameController(Unit unit)
    {
        return Stats.PlayerController == unit.Stats.PlayerController;
    }
}