using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI PlayerTurn;
    public TextMeshProUGUI MovePoints;
    public TextMeshProUGUI SkillPoints;
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
        int skillPoints = player != null ? player.SkillPoints : 0;
        MovePoints.text = movePoints + "/" + Grid.Prep.MovePoints;
        SkillPoints.text = skillPoints + "/" + Grid.Prep.SkillPoints;
    }
}
