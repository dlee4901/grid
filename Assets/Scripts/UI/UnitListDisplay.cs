using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitListDisplay : MonoBehaviour
{
    public GridLayoutGroup container;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        foreach (Unit unit in UnitList.Singleton.units)
        {
            GameObject obj = new GameObject(unit.properties.title);
            UnitHandler unitHandler = obj.AddComponent<UnitHandler>();
            Image image = obj.AddComponent<Image>();
            image.sprite = unit.properties.sprite;
            unitHandler.unit = unit;
            obj.GetComponent<RectTransform>().SetParent(container.transform);
        }
    }
}
