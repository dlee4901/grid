using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit")]
public class Unit : ScriptableObject
{
    [Header("Identifiers")]
    public int _id;
    public string _name;
    public Sprite _sprite;

    [Header("Attributes")]
    public UnitMovement _movement;
    public int _cost;
    public int _maxHealth;
    public int _owner;

    [Header("State")]
    public int _health;
    public int _shield;
    public DirectionFacing _facing;

    // void Init(string _title, UnitMovement _movement, int _playerOwner=0, int _cost=0, int _maxHealth=0, int _currentHealth=0, DirectionFacing _facing=DirectionFacing.N)
    // {
    //     title = _title;
    //     movement = _movement;
    //     playerOwner = _playerOwner;
    //     facing = _facing;
    //     cost = _cost; 
    //     maxHealth = _maxHealth;
    //     currentHealth = _currentHealth;
    // }

    // public static Unit Create(string _title, UnitMovement _movement, int _playerOwner=0, int _cost=0, int _maxHealth=0, int _currentHealth=0, DirectionFacing _facing=DirectionFacing.N)
    // {
    //     var unit = CreateInstance<Unit>();
    //     unit.Init(_title, _movement, _playerOwner, _cost, _maxHealth, _currentHealth, _facing);
    //     return unit;
    // }
}
