using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{
    ImageManager _imageManager;
    InputEventHandler _inputEventHandler;
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
        _inputEventHandler = gameObject.AddComponent<InputEventHandler>();
        _imageManager = gameObject.AddComponent<ImageManager>();
        _imageManager.Init(name, sprite, parent);
        _unitID = unitID;
        _playerController = playerController;
        _listUIPosition = listUIPosition;
    }

    public void OnPropertyChanged(string property)
    {
        if (property == "IsPlaced")
        {
            if (_isPlaced)
            {
                _imageManager.image.color = new Color(0f, 0f, 0f);
            }
            else
            {
                _imageManager.image.color = new Color(1f, 1f, 1f);
            }
        }
    }

    public void UnitUIDrag()
    {
        if (_unitID < UnitList.Singleton.units.Count)
        {
            Unit unit = Instantiate(UnitList.Singleton.units[_unitID]);
            unit.stats.controller = _playerController;
            unit.gameObject.SetActive(true);
            unit.listUIPosition = _listUIPosition;
            unit.isDragging = true;
        }
    }
}