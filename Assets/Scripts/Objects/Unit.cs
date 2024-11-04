using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit")]
public class Unit : ScriptableObject
{
    public string title;
    public UnitMovement movement;
    public int playerOwner;

    public int cost;
    public int maxHealth;
    public int currentHealth;
    public DirectionFacing facing;

    void Init(string _title, UnitMovement _movement, int _playerOwner=0, int _cost=0, int _maxHealth=0, int _currentHealth=0, DirectionFacing _facing=DirectionFacing.N)
    {
        title = _title;
        movement = _movement;
        playerOwner = _playerOwner;
        facing = _facing;
        cost = _cost; 
        maxHealth = _maxHealth;
        currentHealth = _currentHealth;
    }

    public static Unit Create(string _title, UnitMovement _movement, int _playerOwner=0, int _cost=0, int _maxHealth=0, int _currentHealth=0, DirectionFacing _facing=DirectionFacing.N)
    {
        var unit = CreateInstance<Unit>();
        unit.Init(_title, _movement, _playerOwner, _cost, _maxHealth, _currentHealth, _facing);
        return unit;
    }
}
