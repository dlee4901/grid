using UnityEngine;

public class UnitUIManager : MonoBehaviour
{
    private ImageManager _imageManager;
    private UiInputHandler _uiInputHandler;
    private InputHandler _inputHandlerDragDrop;
    private Unit _unit;
    private int _unitID;
    private int _playerController;
    private int _listUIPosition;

    private bool _isPlaced;
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
        _uiInputHandler = gameObject.AddComponent<UiInputHandler>();
        _inputHandlerDragDrop = new InputHandler(InputActionPreset.DragDrop);
        _unit = null;
        _unitID = unitID;
        _playerController = playerController;
        _listUIPosition = listUIPosition;

        transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnPropertyChanged(string property)
    {
        if (property == "IsPlaced")
        {
            if (_isPlaced) _imageManager.SetColor(new Color(0f, 0f, 0f));
            else           _imageManager.SetColor(new Color(1f, 1f, 1f));
        }
    }

    public void UnitCreate()
    {
        if (!IsPlaced && UnitList.Singleton.IsValidUnitID(_unitID))
        {
            _unit = Instantiate(UnitList.Singleton.Units[_unitID]);
            _unit.PlayerController = _playerController;
            _unit.gameObject.SetActive(true);
            _unit.ListUIPosition = _listUIPosition;
        }
    }

    public void UnitDrag()
    {   
        if (!IsPlaced && _unit != null)
        {
            CommandBase command = _inputHandlerDragDrop.HandleInput();
            if (command != null) command.Execute(_unit.gameObject);
        }
    }
}