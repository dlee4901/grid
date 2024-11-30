using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour
{
    public VerticalLayoutGroup Container;
    public Button MoveButton;
    public Button Skill1Button;
    public Button Skill2Button;

    public GridManager Grid;

    void Start()
    {
        MoveButton.onClick.AddListener(() => {
            Grid.SetAvailableTilesSelectedMove();
        });
        Skill1Button.onClick.AddListener(() => {
            
        });
        Skill2Button.onClick.AddListener(() => {
            
        });

        Container.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        Container.gameObject.SetActive(Grid.TileSelected != 0);
    }
}
