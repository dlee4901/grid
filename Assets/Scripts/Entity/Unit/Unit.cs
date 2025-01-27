using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum UnitAction {Move, Skill}

public class Unit : Entity
{
    [Header("Unit")]
    public UnitProperties Properties;
    public Move Move;
    public Skill Skill1;
    public Skill Skill2;
    public Passive Passive;
    
    public int ListUIPosition;

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
        if (unitHealthCounterPrefab != null) InitHealthCounter(unitHealthCounterPrefab);
    }

    private void InitHealthCounter(UnitHealthCounter unitHealthCounterPrefab)
    {
        _healthCounter = Instantiate(unitHealthCounterPrefab, transform);
        _healthCounter.transform.localPosition = new Vector3(1, 1, 0);
        _healthCounter.gameObject.SetActive(false);
    }
}