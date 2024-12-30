using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int GridPosition;
    public DirectionFacing DirectionFacing;
    public int PlayerController;
    public int Health;
    public int HealthOverflow;
    public int DamageDelta;
    public List<int> Counters;

    public Entity() {}

    public void SetPosition(int idx, Vector3Int vec, float tileScale)
    {
        GridPosition = idx;
        transform.position = Util.Get2DWorldPos(vec, tileScale);
        //_healthCounter.gameObject.SetActive(true);
    }

    public void UpdateHealth(int delta=0)
    {
        Health += delta;
        //_healthCounter.CounterText.text = Stats.Health.ToString();
    }

    public bool SameController(Unit unit)
    {
        return PlayerController == unit.PlayerController;
    }
}