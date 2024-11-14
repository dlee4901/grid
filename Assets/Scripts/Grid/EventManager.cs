using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    public event Action<int> TileHoverEvent;
    public event Action<int> UnitDragEvent;
    public event Action<Unit> UnitPlaceEvent;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void StartTileHoverEvent(int id)
    {
        TileHoverEvent?.Invoke(id);
    }

    public void StartUnitDragEvent(int id)
    {
        UnitDragEvent?.Invoke(id);
    }

    public void StartUnitPlaceEvent(Unit unit)
    {
        UnitPlaceEvent?.Invoke(unit);
    }
}
