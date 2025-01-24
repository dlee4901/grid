using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListDisplay : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollView;
    [SerializeField] private GridLayoutGroup _container;

    private List<GridLayoutGroup> _containers;
    private List<List<UnitUIManager>> _unitsList;
    private int _activePlayerList;

    void Start()
    {
        EventManager.Singleton.UnitUIUpdateEvent += UnitUIUpdate;
        InitPlayers(2);
    }

    void InitPlayers(int numPlayers)
    {
        _containers = new List<GridLayoutGroup>{_container};
        _unitsList = new List<List<UnitUIManager>>();
        for (int player = 1; player <= numPlayers; player++)
        {
            List<UnitUIManager> unitManagerList = new List<UnitUIManager>();
            _unitsList.Add(unitManagerList);
            GridLayoutGroup obj = Instantiate(_container, _scrollView.transform);
            _containers.Add(obj);
            InitPlayerContainer(player);
        }
    }

    void InitPlayerContainer(int playerController)
    {
        int listUIPosition = 0;
        foreach (Unit unit in UnitList.Singleton.Units)
        {
            if (unit != null && unit.Properties.Id > 0)
            {
                UnitUIManager unitUIManager = Util.CreateGameObject<UnitUIManager>();
                unitUIManager.Init(unit.Properties.Name, unit.Properties.Sprite, _containers[playerController].transform, unit.Properties.Id, playerController, listUIPosition);
                _unitsList[playerController-1].Add(unitUIManager);
                listUIPosition += 1;
            }
        }
        _containers[playerController].gameObject.SetActive(false);
    }

    void UnitUIUpdate(int playerController, int listUIPosition, bool placed)
    {
        _unitsList[playerController-1][listUIPosition].IsPlaced = placed;
    }

    public void SetActivePlayerList(int playerController)
    {
        if (playerController < _containers.Count)
        {
            _containers[_activePlayerList].gameObject.SetActive(false);
            _scrollView.content = _containers[playerController].GetComponent<RectTransform>();
            _containers[playerController].gameObject.SetActive(true);
            _activePlayerList = playerController;
        }
    }
}
