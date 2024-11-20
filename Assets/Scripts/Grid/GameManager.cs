using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    public GridManager grid;
    public CinemachineCameraManager cam;

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

    // Update is called once per frame
    void Update()
    {
        StartTurn();
    }

    void Init()
    {
        cam.Init(grid.x, grid.y);
    }

    void PlaceUnits()
    {
        
    }

    void StartTurn()
    {
        if (_turn == 0 || _turn >= grid.numPlayers)
        {
            _turn = 1;
        }
        else
        {
            _turn += 1;
        }
    }
}
