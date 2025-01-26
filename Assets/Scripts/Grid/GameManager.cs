using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineCameraManager _cam;
    [SerializeField] private GridManager _grid;
    [SerializeField] private UIManager _uiManager;

    private List<Player> _players;
    private List<float> _times;
    private int _turn;
    private int _timeControl;

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
        _cam.Init(_grid);
    }

    public void StartPlacement(int playerController)
    {
        _uiManager.StartPlacement(playerController);
        _grid.SetAvailableTilesPlacement(playerController);
    }

    public void StartGame()
    {
        _uiManager.StartGame();
        _grid.StartGame();
    }
}
