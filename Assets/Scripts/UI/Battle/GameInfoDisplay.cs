using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI PlayerTurn;
    public TextMeshProUGUI MovePoints;
    public TextMeshProUGUI Mana;
    public Button EndTurn;

    public GridManager Grid;

    // Update is called once per frame
    void Start()
    {
        EndTurn.onClick.AddListener(() => {
            Grid.NextTurn();
        });
    }

    public void UpdateDisplay()
    {
        PlayerTurn.text = "Player " + Grid.PlayerTurn + " Turn";
        Player player = Grid.GetActivePlayer();
        int movePoints = player != null ? player.MovePoints : 0;
        int mana = player != null ? player.Mana : 0;
        MovePoints.text = movePoints + "/" + Grid.Prep.MovePoints;
        Mana.text = mana + "/" + Grid.Prep.Mana;
    }
}
