using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    public CinemachineCameraManager cam;
    public GridManager grid;
    public UnitListDisplay unitListDisplay;

    List<Player> _players;
    List<float> _times;
    int _turn;
    int _timeControl;

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
        Init();
    }

    void Update()
    {
        // PlaceUnits();
        // StartTurn();
    }

    void Init()
    {
        cam.Init(grid.x, grid.y);
    }

    public void StartPlacement(int playerController)
    {
        unitListDisplay.SetActivePlayerList(playerController);
        grid.SetAvailableTilesPlacement(playerController);
    }

    public void StartGame()
    {
        unitListDisplay.gameObject.SetActive(false);
        grid.StartGame();
    }
}
