using UnityEngine;
using UnityEngine.EventSystems;

// Handles inputs for UI
public class EventSystemHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    UnitUIManager _unitUIManager;

    void Start()
    {
        if (gameObject.tag == "UI Unit")
        {
            _unitUIManager = gameObject.GetComponent<UnitUIManager>();
        }
    }

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
            _unitUIManager.UnitCreate();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.tag == "UI Unit")
        {
            _unitUIManager.UnitDrag();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameObject.tag == "UI Unit")
        {
            _unitUIManager.UnitDrag();
        }
    }
}
