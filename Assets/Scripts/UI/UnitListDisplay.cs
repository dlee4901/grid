using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class UnitListDisplay : MonoBehaviour
{
    public ScrollRect scrollView;
    public GridLayoutGroup container;

    List<GridLayoutGroup> _containers;
    int _activePlayerList;

    void Start()
    {
        InitPlayers(2);
        SetActivePlayerList(1);
    }

    void InitPlayers(int numPlayers)
    {
        _containers = new List<GridLayoutGroup>{container};
        _activePlayerList = 0;
        for (int player = 1; player <= numPlayers; player++)
        {
            GridLayoutGroup obj = Instantiate(container, scrollView.transform);
            _containers.Add(obj);
            InitPlayerContainer(player);
        }
    }

    void InitPlayerContainer(int player)
    {
        foreach (Unit unit in UnitList.Singleton.units)
        {
            if (unit.properties.id > 0)
            {
                GameObject imageManagerGO = new GameObject();
                imageManagerGO.tag = "UI Unit";
                ImageManager imageManager = imageManagerGO.AddComponent<ImageManager>();
                imageManager.Init(unit.properties.title, unit.properties.sprite, _containers[player].transform, unit.properties.id);
            }
        }
        _containers[player].gameObject.SetActive(false);
    }

    public void SetActivePlayerList(int player)
    {
        if (player < _containers.Count)
        {
            _containers[_activePlayerList].gameObject.SetActive(false);
            scrollView.content = _containers[player].GetComponent<RectTransform>();
            _containers[player].gameObject.SetActive(true);
            _activePlayerList = player;
        }
    }
}
