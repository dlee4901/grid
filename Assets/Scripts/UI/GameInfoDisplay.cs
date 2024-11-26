using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoDisplay : MonoBehaviour
{
    public GridManager grid;
    public TextMeshProUGUI playerTurn;
    public Button endTurn;

    // Update is called once per frame
    void Start()
    {
        endTurn.onClick.AddListener(() => {
            Debug.Log("End Turn Clicked");
            EventManager.Singleton.StartEndTurnEvent();
        });
    }

    public void SetPlayerTurn(int player)
    {
        playerTurn.text = "Player " + player + " Turn";
    }
}
