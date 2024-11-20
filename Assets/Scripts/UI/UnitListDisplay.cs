using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class UnitListDisplay : MonoBehaviour
{
    public ScrollRect scrollView;
    public GridLayoutGroup container;

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
        _containers = new List<GridLayoutGroup>{container};
        _unitsList = new List<List<UnitUIManager>>();
        for (int player = 1; player <= numPlayers; player++)
        {
            List<UnitUIManager> unitManagerList = new List<UnitUIManager>();
            _unitsList.Add(unitManagerList);
            GridLayoutGroup obj = Instantiate(container, scrollView.transform);
            _containers.Add(obj);
            InitPlayerContainer(player);
        }
    }

    void InitPlayerContainer(int playerController)
    {
        int listUIPosition = 0;
        foreach (Unit unit in UnitList.Singleton.units)
        {
            if (unit.properties.id > 0)
            {
                UnitUIManager unitUIManager = Util.CreateGameObject<UnitUIManager>();
                unitUIManager.Init(unit.properties.title, unit.properties.sprite, _containers[playerController].transform, unit.properties.id, playerController, listUIPosition);
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
            scrollView.content = _containers[playerController].GetComponent<RectTransform>();
            _containers[playerController].gameObject.SetActive(true);
            _activePlayerList = playerController;
        }
    }

    void UnitUIUpdate(int playerController, int listUIPosition, bool placed)
    {
        _unitsList[playerController-1][listUIPosition].IsPlaced = placed;
    }
}
