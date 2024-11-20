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

    void Awake()
    {
        //EventManager.Singleton.UnitUIDragEvent += UnitUIDrag;
    }

    public void Init(string name, Sprite sprite, Transform parent, int unitID, int playerController, int listUIPosition)
    {
        _imageManager = gameObject.AddComponent<ImageManager>();
        _imageManager.Init(name, sprite, parent);
        _inputEventHandler = gameObject.AddComponent<InputEventHandler>();
        //_inputEventHandler.InitUIUnit(unitID, playerController, listUIPosition);
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
                Debug.Log("IsPlaced true");
                Debug.Log(_imageManager.image);
                gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f);
            }
            else
            {
                Debug.Log("IsPlaced false");
                Debug.Log(_imageManager.image);
                gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
        }
    }

    public void UnitUIDrag()
    {
        if (_unitID < UnitList.Singleton.units.Count)
        {
            Unit unit = Instantiate(UnitList.Singleton.units[_unitID]);
            unit.properties.controller = _playerController;
            unit.gameObject.SetActive(true);
            unit.listUIPosition = _listUIPosition;
            unit.isDragging = true;
        }
    }
}