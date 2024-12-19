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
    public List<UnitMove> Moves;
    public UnitStats Stats;

    public int ListUIPosition;
    //public bool IsDragging;

    SpriteRenderer _spriteRenderer;
    UnitHealthCounter _healthCounter;

    public void Init(UnitHealthCounter unitHealthCounterPrefab)
    {
        name = Properties.Name;
        gameObject.SetActive(false);
        transform.localScale = new Vector3(15f, 15f, 0f);

        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Properties.Sprite;
        _spriteRenderer.sortingLayerName = "Unit";
        _healthCounter = Instantiate(unitHealthCounterPrefab, transform);
        _healthCounter.transform.localPosition = new Vector3(1, 1, 0);
        _healthCounter.gameObject.SetActive(false);
    }

    public void SetPosition(int idx, Vector3Int vec, float tileScale)
    {
        Stats.GridPosition = idx;
        transform.position = Util.Get2DWorldPos(vec, tileScale);
        _healthCounter.gameObject.SetActive(true);
    }

    public void UpdateHealth(int delta=0)
    {
        Stats.Health += delta;
        _healthCounter.CounterText.text = Stats.Health.ToString();
    }

    public bool SameController(Unit unit)
    {
        return Stats.PlayerController == unit.Stats.PlayerController;
    }
}