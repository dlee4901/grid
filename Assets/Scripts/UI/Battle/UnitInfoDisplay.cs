using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour
{
    public Image UnitImage;
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

    public void UpdateDisplay(Entity entity=null)       
    {
        Unit unit = null;
        if (entity is Unit) unit = (Unit)entity;
        if (unit == null || unit.PlayerController != Grid.PlayerTurn) 
        {
            Container.gameObject.SetActive(false);
        }
        else
        {
            UnitImage.sprite = unit.Properties.Sprite;
            Player player = Grid.GetActivePlayer();
            MoveButton.interactable = player?.MovePoints > 0;
            Skill1Button.interactable = player?.SkillPoints > 0;
            Skill2Button.interactable = player?.SkillPoints > 0;
            Container.gameObject.SetActive(true);
        }
    }
}
