using UnityEngine;
using UnityEngine.UI;

public class LocalTest : MonoBehaviour
{
    public Button startGame;
    public Button player1;
    public Button player2;
    public UnitListDisplay unitListDisplay;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        startGame.onClick.AddListener(() => {
            Debug.Log("Start Game Clicked");
        });
        player1.onClick.AddListener(() => {
            Debug.Log("Player 1 Clicked");
            unitListDisplay.SetActivePlayerList(1);
            gameManager.grid.SetInitialUnitSpawns(1);
        });
        player2.onClick.AddListener(() => {
            Debug.Log("Player 2 Clicked");
            unitListDisplay.SetActivePlayerList(2);
            gameManager.grid.SetInitialUnitSpawns(2);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnClick()
    {

    }
}
