using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerTurn;
    [SerializeField] private TextMeshProUGUI MovePoints;
    [SerializeField] private TextMeshProUGUI Mana;
    [SerializeField] private Button EndTurn;

    // Update is called once per frame
    void Start()
    {
        EndTurn.onClick.AddListener(() => {
            EventManager.Singleton.StartGameInfoDisplayEndTurnEvent();
        });
    }

    public void UpdateDisplay(GridManager grid)
    {
        PlayerTurn.text = "Player " + grid.PlayerTurn + " Turn";
        Player player = grid.GetActivePlayer();
        int movePoints = player != null ? player.MovePoints : 0;
        int mana = player != null ? player.Mana : 0;
        MovePoints.text = movePoints + "/" + grid.MovePoints;
        Mana.text = mana + "/" + grid.Mana;
    }
}
