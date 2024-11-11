using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitListDisplay : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GridLayoutGroup container;

    void Start()
    {
        EventManager.Singleton.UnitDragEvent += UnitDrag;
    }

    public void Init()
    {
        foreach (Unit unit in UnitList.Singleton.units)
        {
            GameObject obj = new GameObject(unit.name);
            UnitHandler unitHandler = obj.AddComponent<UnitHandler>();
            Image image = obj.AddComponent<Image>();
            image.sprite = unit._sprite;
            obj.GetComponent<RectTransform>().SetParent(container.transform);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("list OnPointerDown");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("list OnDrag");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("list OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("list OnEndDrag");
    }

    void UnitDrag(Unit unit)
    {
        //GameObject obj = new GameObject(id);

    }
}
