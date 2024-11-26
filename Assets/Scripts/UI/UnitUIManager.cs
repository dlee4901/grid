using UnityEngine;
using UnityEngine.InputSystem;

public class UnitUIManager : MonoBehaviour
{
    ImageManager _imageManager;
    EventSystemHandler _eventSystemHandler;
    UnitInputHandler _inputHandler;
    Unit _unit;
    int _unitID;
    int _playerController;
    int _listUIPosition;

    bool _isPlaced;
    public bool IsPlaced
    {
        get { return _isPlaced; }
        set { _isPlaced = value; OnPropertyChanged("IsPlaced"); }
    }

    public void Init(string name, Sprite sprite, Transform parent, int unitID, int playerController, int listUIPosition)
    {
        tag = "UI Unit";
        _inputHandler = new UnitInputHandler(InputSystem.actions.FindAction("Player/Select"));
        _eventSystemHandler = gameObject.AddComponent<EventSystemHandler>();
        _imageManager = gameObject.AddComponent<ImageManager>();
        _eventSystemHandler.Init(this);
        _imageManager.Init(name, sprite, parent);
        _unitID = unitID;
        _playerController = playerController;
        _listUIPosition = listUIPosition;
    }

    public void OnPropertyChanged(string property)
    {
        if (property == "IsPlaced")
        {
            if (_isPlaced) _imageManager.image.color = new Color(0f, 0f, 0f);
            else           _imageManager.image.color = new Color(1f, 1f, 1f);
        }
    }

    public void UnitUICreate()
    {
        if (UnitList.Singleton.IsValidUnitID(_unitID))
        {
            _unit = Instantiate(UnitList.Singleton.units[_unitID]);
            _unit.stats.controller = _playerController;
            _unit.gameObject.SetActive(true);
            _unit.listUIPosition = _listUIPosition;
            //unit.isDragging = true;
            
        }
    }

    public void UnitUIDrag()
    {
        if (_unit != null)
        {
            ActionBase action = _inputHandler.HandleInput(_unit);
            if (action != null) action.Execute();
        }
    }
}