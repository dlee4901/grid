using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CinemachineCameraManager Cam;
    public GridManager Grid;
    public UIManager UiManager;

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
        Cam.Init(Grid.X, Grid.Y);
    }

    public void StartPlacement(int playerController)
    {
        UiManager.StartPlacement(playerController);
        Grid.SetAvailableTilesPlacement(playerController);
    }

    public void StartGame()
    {
        UiManager.StartGame();
        Grid.StartGame();
    }
}
