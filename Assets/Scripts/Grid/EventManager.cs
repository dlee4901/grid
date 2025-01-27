using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    public event Action<int> TileHoverEvent;
    public event Action<Unit> UnitDragEvent;
    public event Action<Unit> UnitPlaceEvent;
    public event Action<int, int, bool> UnitUIUpdateEvent;

    public event Action GameInfoDisplayEndTurnEvent;
    public event Action UnitInfoDisplayMoveEvent;

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

    public void StartGameInfoDisplayEndTurnEvent()
    {
        GameInfoDisplayEndTurnEvent?.Invoke();
    }

    public void StartUnitInfoDisplayMoveEvent()
    {
        UnitInfoDisplayMoveEvent?.Invoke();
    }
}
