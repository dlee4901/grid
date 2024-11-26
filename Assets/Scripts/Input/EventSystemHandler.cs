using UnityEngine;
using UnityEngine.EventSystems;

// Handles inputs for UI
public class EventSystemHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    UnitUIManager _unitUIManager;

    public void Init(UnitUIManager unitUIManager)
    {
        _unitUIManager = unitUIManager;
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
            if (!_unitUIManager.IsPlaced)
            {
                _unitUIManager.UnitUICreate();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _unitUIManager.UnitUIDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _unitUIManager.UnitUIDrag();
    }
}
