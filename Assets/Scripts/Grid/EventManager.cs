using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    public event Action<int> TileHover;
    public event Action<Unit> UnitDrag;
    public event Action<Unit> UnitPlace;
    public event Action<int, int, bool> UnitUIUpdate;

    public event Action GameInfoDisplayEndTurn;
    public event Action UnitInfoDisplayMove;

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

    public void StartTileHover(int id)
    {
        TileHover?.Invoke(id);
    }

    public void StartUnitDrag(Unit unit)
    {
        UnitDrag?.Invoke(unit);
    }

    public void StartUnitPlace(Unit unit)
    {
        UnitPlace?.Invoke(unit);
    }

    public void StartUnitUIUpdate(int playerController, int listUIPosition, bool placed)
    {
        UnitUIUpdate?.Invoke(playerController, listUIPosition, placed);
    }

    public void StartGameInfoDisplayEndTurn()
    {
        GameInfoDisplayEndTurn?.Invoke();
    }

    public void StartUnitInfoDisplayMove()
    {
        UnitInfoDisplayMove?.Invoke();
    }
}
