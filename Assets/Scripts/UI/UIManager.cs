using UnityEngine;

public class UIManager : MonoBehaviour
{
    public LocalTest LocalTest;
    public UnitListDisplay UnitListDisplay;
    
    public GameInfoDisplay GameInfoDisplay;
    public UnitInfoDisplay UnitInfoDisplay;

    void Awake()
    {
        LocalTest.gameObject.SetActive(true);
        UnitListDisplay.gameObject.SetActive(true);
        GameInfoDisplay.gameObject.SetActive(false);
        UnitInfoDisplay.gameObject.SetActive(false);
    }

    public void StartPlacement(int playerController)
    {
        UnitListDisplay.SetActivePlayerList(playerController);
    }

    public void StartGame()
    {
        LocalTest.gameObject.SetActive(false);
        UnitListDisplay.gameObject.SetActive(false);
        GameInfoDisplay.gameObject.SetActive(true);
        UnitInfoDisplay.gameObject.SetActive(true);
    }
}
