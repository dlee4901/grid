using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineCameraController _camera;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SquareGrid _grid;

    private List<Player> _players;
    private List<float> _times;
    private int _turn;
    private int _timeControl;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        // PlaceUnits();
        // StartTurn();
        
    }

    private void Init()
    {
        _grid.Init(_uiManager);
        _camera.Init(_grid);
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
