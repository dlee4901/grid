using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    public event Action<int> TileHoverEvent;
    public event Action<Unit> UnitDragEvent;
    public event Action<GameObject> UnitPlaceEvent;

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

    public void StartUnitDragEvent(Unit unit)
    {
        UnitDragEvent?.Invoke(unit);
    }

    public void StartUnitPlaceEvent(GameObject unit)
    {
        UnitPlaceEvent?.Invoke(unit);
    }
}
