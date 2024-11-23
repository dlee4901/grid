using UnityEngine;
using UnityEngine.UI;

public class GameInfoDisplay : MonoBehaviour
{
    public GridManager grid;
    public Text playerTurn;

    public void SetPlayerTurn(int player)
    {
        playerTurn.text = "Player " + player + " Turn";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
