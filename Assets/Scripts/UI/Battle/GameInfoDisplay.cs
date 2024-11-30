using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI PlayerTurn;
    public Button EndTurn;

    public GridManager Grid;

    // Update is called once per frame
    void Start()
    {
        EndTurn.onClick.AddListener(() => {
            Grid.NextTurn();
        });
    }

    public void SetPlayerTurn(int player)
    {
        PlayerTurn.text = "Player " + player + " Turn";
    }
}
