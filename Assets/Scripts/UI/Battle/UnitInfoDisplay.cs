using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour
{
    public Button MoveButton;
    public Button Skill1Button;
    public Button Skill2Button;
    public VerticalLayoutGroup Container;
    public GridManager Grid;

    UiInputHandler _uiInputHandler;

    void Start()
    {
        MoveButton.onClick.AddListener(() => {
            Grid.DisplayAction(UnitAction.Move);
        });
        Container.gameObject.SetActive(false);
        _uiInputHandler = gameObject.AddComponent<UiInputHandler>();
    }

    public void ShowUnitActions(Unit unit)       
    {
        Container.gameObject.SetActive(unit != null);
    }
}
