using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct UnitStats
{
    public int position;
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

    UnitInputHandler _inputHandler;
    InputAction _selectAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_selectAction = InputSystem.actions.FindAction("Player/Select");
        _inputHandler = new UnitInputHandler(InputSystem.actions.FindAction("Player/Select"));
    }

    // Update is called once per frame
    void Update()
    {
        //HandleDrag();
        ActionBase action = _inputHandler.HandleInput(this);
        if (action != null) action.Execute();
    }

    public void Init()
    {
        name = properties.title;
        gameObject.SetActive(false);
        transform.localScale = new Vector3(10f, 10f, 0f);

        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = properties.sprite;
        _spriteRenderer.sortingLayerName = "Unit";
    }

    public void HoldUnit(Vector3 mousePosition)
    {
        Debug.Log(mousePosition);
        transform.localPosition = mousePosition;
    }

    public void PlaceUnit(int positionIdx)
    {
        EventManager.Singleton.StartUnitPlaceEvent(this, listUIPosition);
    }

    // void HandleDrag()
    // {
    //     if (isDragging)
    //     {
    //         transform.localPosition = Util.GetMousePosition();
    //         transform.localScale = new Vector3(10f, 10f, 0f);
    //         if (_selectAction.WasReleasedThisFrame())
    //         {
    //             Debug.Log("selectAction released");
    //             EventManager.Singleton.StartUnitPlaceEvent(this, listUIPosition);
    //             isDragging = false;
    //         }
    //     }
    // }
}