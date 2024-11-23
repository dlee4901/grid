using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CinemachineCameraManager cam;
    public GridManager grid;
    public UIManager uiManager;

    List<Player> _players;
    List<float> _times;
    int _turn;
    int _timeControl;

    void Awake()
    {
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
        uiManager.StartPlacement(playerController);
        grid.SetAvailableTilesPlacement(playerController);
    }

    public void StartGame()
    {
        uiManager.StartGame();
        grid.StartGame();
    }
}
