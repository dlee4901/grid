using UnityEngine;
using UnityEngine.InputSystem;

public class UnitUIManager : MonoBehaviour
{
    ImageManager _imageManager;
    EventSystemHandler _eventSystemHandler;
    DragDropInputHandler _inputHandler;
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
        _imageManager = gameObject.AddComponent<ImageManager>();
        _imageManager.Init(name, sprite, parent);
        _eventSystemHandler = gameObject.AddComponent<EventSystemHandler>();
        _inputHandler = new DragDropInputHandler();
        _unit = null;
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

    public void UnitCreate()
    {
        if (!IsPlaced && UnitList.Singleton.IsValidUnitID(_unitID))
        {
            _unit = Instantiate(UnitList.Singleton.units[_unitID]);
            _unit.stats.controller = _playerController;
            _unit.gameObject.SetActive(true);
            _unit.listUIPosition = _listUIPosition;
        }
    }

    public void UnitDrag()
    {   
        if (!IsPlaced && _unit != null)
        {
            ActionBase action = _inputHandler.HandleInput();
            if (action != null) action.Execute(_unit.gameObject);
        }
    }
}