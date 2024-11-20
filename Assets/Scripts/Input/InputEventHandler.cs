using UnityEngine;
using UnityEngine.EventSystems;

public class InputEventHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    int _unitID;
    int _playerController;

    public void InitUIUnit(int unitID, int playerController, int listUIPosition)
    {
        _unitID = unitID;
        _playerController = playerController;
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
            UnitUIManager unitUIManagerObj = gameObject.GetComponent<UnitUIManager>();
            if (!unitUIManagerObj.IsPlaced)
            {
                unitUIManagerObj.UnitUIDrag();
            }
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
