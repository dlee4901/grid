using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnitHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Unit unit;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject obj = new GameObject(unit.properties.title);
        UnitManager unitManager = obj.AddComponent<UnitManager>();
        unitManager.unit = unit;
        unitManager.isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        //gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }
}
