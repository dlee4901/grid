using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    InputAction _selectAction;

    public UnitProperties properties;

    public int listUIPosition;
    public bool isDragging;

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

    public void Init(UnitProperties properties_, Transform parent=null)
    {
        properties = properties_;
        if (parent) transform.parent = parent;
        name = properties.title;
        gameObject.SetActive(false);

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
                EventManager.Singleton.StartUnitPlaceEvent(this, listUIPosition);
                isDragging = false;
            }
        }
    }
}