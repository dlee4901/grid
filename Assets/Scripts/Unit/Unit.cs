using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public UnitProperties properties;

    public bool isDragging;

    SpriteRenderer _spriteRenderer;
    InputAction _selectAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _selectAction = InputSystem.actions.FindAction("Player/Select");
    }

    // Update is called once per frame
    void Update()
    {
        HandleDrag();
    }

    public void Init()
    {
        name = properties.title;
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = properties.sprite;
        _spriteRenderer.sortingLayerName = "Unit";
    }

    void HandleDrag()
    {
        if (isDragging)
        {
            transform.localPosition = Util.GetMousePosition(true);
            transform.localScale = new Vector3(10f, 10f, 0f);
            if (_selectAction.WasPerformedThisFrame())
            {
                EventManager.Singleton.StartUnitPlaceEvent(this);
                isDragging = false;
            }
        }
    }
}