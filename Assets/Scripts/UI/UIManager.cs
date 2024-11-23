using UnityEngine;

public class UIManager : MonoBehaviour
{
    public LocalTest localTest;
    public UnitListDisplay unitListDisplay;
    
    public GameInfoDisplay gameInfoDisplay;
    public UnitInfoDisplay unitInfoDisplay;

    void Awake()
    {
        localTest.gameObject.SetActive(true);
        unitListDisplay.gameObject.SetActive(true);
        gameInfoDisplay.gameObject.SetActive(false);
        unitInfoDisplay.gameObject.SetActive(false);
    }

    public void StartPlacement(int playerController)
    {
        unitListDisplay.SetActivePlayerList(playerController);
    }

    public void StartGame()
    {
        localTest.gameObject.SetActive(false);
        unitListDisplay.gameObject.SetActive(false);
        gameInfoDisplay.gameObject.SetActive(true);
        unitInfoDisplay.gameObject.SetActive(true);
    }
}
