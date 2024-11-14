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
        
    }

    void Init()
    {
        cam.Init(grid.x, grid.y);
    }
}
