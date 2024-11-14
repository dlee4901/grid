using UnityEngine;
using UnityEngine.EventSystems;

public class EventHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int id;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.tag == "UI Unit")
        {
            EventManager.Singleton.StartUnitDragEvent(id);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        //gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }
}
