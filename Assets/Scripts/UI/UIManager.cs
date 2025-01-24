using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LocalTest _localTest;
    [SerializeField] private UnitListDisplay _unitListDisplay;
    
    [SerializeField] private GameInfoDisplay _gameInfoDisplay;
    [SerializeField] private UnitInfoDisplay _unitInfoDisplay;

    void Awake()
    {
        _localTest.gameObject.SetActive(true);
        _unitListDisplay.gameObject.SetActive(true);
        _gameInfoDisplay.gameObject.SetActive(false);
        _unitInfoDisplay.gameObject.SetActive(false);
    }

    public void StartPlacement(int playerController)
    {
        _unitListDisplay.SetActivePlayerList(playerController);
    }

    public void StartGame()
    {
        _localTest.gameObject.SetActive(false);
        _unitListDisplay.gameObject.SetActive(false);
        _gameInfoDisplay.gameObject.SetActive(true);
        _unitInfoDisplay.gameObject.SetActive(true);
    }
}
