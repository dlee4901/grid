using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraSystem : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        Debug.Log(Mouse.current.position);
        // if (ctx.started) _origin = GetMousePosition;
        // _tileClicked = (ctx.started || _tileClicked) && _grid._tileHovered != -1;
        // _isDragging = ctx.started || ctx.performed;
    }

    // public void OnZoom(InputAction.CallbackContext ctx)
    // {
    //     Debug.Log("onzoom");
    //     if (ctx.started) _origin = GetMousePosition;
    //     _scrollAmount = ctx.ReadValue<float>();
    //     _isZooming = ctx.started || ctx.performed;
    // }
}
