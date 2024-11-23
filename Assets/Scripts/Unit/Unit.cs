using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct UnitStats
{
    public int controller;
    public int health;
    public int shield;
    public DirectionFacing facing;
}

public class Unit : MonoBehaviour
{
    public UnitMovement movement;
    public UnitProperties properties;
    public UnitStats stats;

    public int listUIPosition;
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

    public void Init(Transform parent=null)
    {
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
            if (_selectAction.WasReleasedThisFrame())
            {
                Debug.Log("selectAction released");
                EventManager.Singleton.StartUnitPlaceEvent(this, listUIPosition);
                isDragging = false;
            }
        }
    }
}