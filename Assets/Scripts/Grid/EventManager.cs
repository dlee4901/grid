using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    public event Action<int> TileHoverEvent;
    public event Action<Unit, int> UnitPlaceEvent;
    public event Action<int, int, bool> UnitUIUpdateEvent;

    void Awake()
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

    public void StartUnitPlaceEvent(Unit unit, int listUIPosition)
    {
        UnitPlaceEvent?.Invoke(unit, listUIPosition);
    }

    public void StartUnitUIDragEvent(int playerController, int listUIPosition, bool placed)
    {
        UnitUIUpdateEvent?.Invoke(playerController, listUIPosition, placed);
    }
}
