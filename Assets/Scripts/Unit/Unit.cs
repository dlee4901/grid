using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct UnitStats
{
    public int position;
    public int controller;
    public int health;
    public int shield;
    public DirectionFacing facing;
}

public class Unit : MonoBehaviour
{
    public UnitProperties properties;
    public List<UnitMovement> movement;
    public UnitStats stats;

    public int listUIPosition;
    public bool isDragging;

    SpriteRenderer _spriteRenderer;

    public void Init()
    {
        name = properties.title;
        gameObject.SetActive(false);
        transform.localScale = new Vector3(10f, 10f, 0f);

        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = properties.sprite;
        _spriteRenderer.sortingLayerName = "Unit";
    }

    public void SetPosition(int idx, Vector3Int vec, float tileScale)
    {
        stats.position = idx;
        transform.position = Util.Get2DWorldPos(vec, tileScale);
    }
}