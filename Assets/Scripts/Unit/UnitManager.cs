using UnityEngine;
using UnityEngine.InputSystem;

public class UnitManager : MonoBehaviour
{
    public Unit unit;
    public bool isDragging;

    SpriteRenderer _spriteRenderer;
    InputAction _selectAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = unit.properties.sprite;
        _spriteRenderer.sortingLayerName = "Unit";
        _selectAction = InputSystem.actions.FindAction("Player/Select");
    }

    // Update is called once per frame
    void Update()
    {
        HandleDrag();
    }

    void HandleDrag()
    {
        if (isDragging)
        {
            gameObject.transform.localPosition = Util.GetMousePosition(true);
            gameObject.transform.localScale = new Vector3(10f, 10f, 0f);
            if (_selectAction.WasPerformedThisFrame())
            {
                Debug.Log("SelectAction");
                EventManager.Singleton.StartUnitPlaceEvent(gameObject);
                isDragging = false;
            }
        }
    }
}
