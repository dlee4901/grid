using UnityEngine;

[CreateAssetMenu(menuName = "Unit")]
public class Unit : ScriptableObject
{
    public UnitProperties properties;
    public UnitMovement movement;
}