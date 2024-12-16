using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListDisplay : MonoBehaviour
{
    public ScrollRect ScrollView;
    public GridLayoutGroup Container;

    List<GridLayoutGroup> _containers;
    List<List<UnitUIManager>> _unitsList;
    int _activePlayerList;

    void Start()
    {
        EventManager.Singleton.UnitUIUpdateEvent += UnitUIUpdate;
        InitPlayers(2);
    }

    void InitPlayers(int numPlayers)
    {
        _containers = new List<GridLayoutGroup>{Container};
        _unitsList = new List<List<UnitUIManager>>();
        for (int player = 1; player <= numPlayers; player++)
        {
            List<UnitUIManager> unitManagerList = new List<UnitUIManager>();
            _unitsList.Add(unitManagerList);
            GridLayoutGroup obj = Instantiate(Container, ScrollView.transform);
            _containers.Add(obj);
            InitPlayerContainer(player);
        }
    }

    void InitPlayerContainer(int playerController)
    {
        int listUIPosition = 0;
        foreach (Unit unit in UnitList.Singleton.units)
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

    public void SetActivePlayerList(int playerController)
    {
        if (playerController < _containers.Count)
        {
            _containers[_activePlayerList].gameObject.SetActive(false);
            ScrollView.content = _containers[playerController].GetComponent<RectTransform>();
            _containers[playerController].gameObject.SetActive(true);
            _activePlayerList = playerController;
        }
    }

    void UnitUIUpdate(int playerController, int listUIPosition, bool placed)
    {
        _unitsList[playerController-1][listUIPosition].IsPlaced = placed;
    }
}
