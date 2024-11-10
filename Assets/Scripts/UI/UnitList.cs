using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitList : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Canvas canvas;
    public GridLayoutGroup list;
    public List<Unit> units;

    void Start()
    {
        foreach (Unit unit in units)
        {
            GameObject obj = new GameObject(unit.name);
            obj.AddComponent<UnitHandler>();
            Image image = obj.AddComponent<Image>();
            image.sprite = unit.sprite;
            obj.GetComponent<RectTransform>().SetParent(list.transform);
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
}
