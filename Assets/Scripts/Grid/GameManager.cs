using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager grid;
    public CameraManager camera;
    List<Player> players;
    List<float> times;
    int turn;
    int timeControl;

    // Start is called before the first frame update
    void Start()
    {
        camera._minX = 0;
        camera._minY = 0;
        camera._maxX = 10 * (grid._x - 1);
        camera._maxY = 10 * (grid._y - 1);
        camera._minZoom = 10f;
        camera._maxZoom = Mathf.Max(grid._x, grid._y) * 10f;
        camera._zoomMultiplier = (camera._maxZoom / camera._minZoom) / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
