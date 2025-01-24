using UnityEngine;
using UnityEngine.EventSystems;

// Handles UI inputs
public class UiInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //public bool IsMouseOver;

    private UnitUIManager _unitUIManager;

    void Start()
    {
        if (gameObject.tag == "UI Unit")
        {
            _unitUIManager = gameObject.GetComponent<UnitUIManager>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //IsMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //IsMouseOver = false;
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
