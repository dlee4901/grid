using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    public event Action EndTurnEvent;
    public event Action<int> TileHoverEvent;
    public event Action<Unit> UnitDragEvent;
    public event Action<Unit> UnitPlaceEvent;
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

    public void StartEndTurnEvent()
    {
        EndTurnEvent?.Invoke();
    }

    public void StartTileHoverEvent(int id)
    {
        TileHoverEvent?.Invoke(id);
    }

    public void StartUnitDragPlacement(Unit unit)
    {
        UnitDragEvent?.Invoke(unit);
    }

    public void StartUnitPlaceEvent(Unit unit)
    {
        UnitPlaceEvent?.Invoke(unit);
    }

    public void StartUnitUIUpdateEvent(int playerController, int listUIPosition, bool placed)
    {
        UnitUIUpdateEvent?.Invoke(playerController, listUIPosition, placed);
    }
}
