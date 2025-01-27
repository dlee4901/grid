using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour
{
    [SerializeField] private Image _unitImage;
    [SerializeField] private Button _moveButton;
    [SerializeField] private Button _skill1Button;
    [SerializeField] private Button _skill2Button;
    [SerializeField] private VerticalLayoutGroup _container;

    private UiInputHandler _uiInputHandler;

    private void Start()
    {
        _moveButton.onClick.AddListener(() => {
            EventManager.Singleton.StartUnitInfoDisplayMoveEvent();
            //_grid.DisplayAction(UnitAction.Move);
        });
        _container.gameObject.SetActive(false);
        _uiInputHandler = gameObject.AddComponent<UiInputHandler>();
    }

    public void UpdateDisplay(GridManager grid, Entity entity=null)       
    {
        Unit unit = null;
        if (entity is Unit) unit = (Unit)entity;
        if (unit == null || unit.PlayerController != grid.PlayerTurn) 
        {
            _container.gameObject.SetActive(false);
        }
        else
        {
            _unitImage.sprite = unit.Properties.Sprite;
            Player player = grid.GetActivePlayer();
            _moveButton.interactable = player?.MovePoints > 0;
            _skill1Button.interactable = player?.Mana > 0;
            _skill2Button.interactable = player?.Mana > 0;
            _container.gameObject.SetActive(true);
        }
    }
}
