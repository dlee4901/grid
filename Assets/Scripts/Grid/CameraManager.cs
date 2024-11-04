using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    Camera _cam;
    Vector3 _origin;
    Vector3 _difference;
    bool _tileClicked;
    bool _isDragging;
    bool _isZooming;
    float _scrollAmount;
    float _height;
    float _width;
    float _zoom;

    public float _zoomMultiplier = 0.1f;

    float _minZoom = 2f;
    float _maxZoom = 8f;
    float _velocity = 0f;
    float _smoothTime = 0.25f;

    [SerializeField] GridManager _grid;

    void Awake()
    {
        _cam = Camera.main;
        _zoom = _cam.orthographicSize;
        _height = 2f * _cam.orthographicSize;
        _width = _height * _cam.aspect;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _tileClicked = (ctx.started || _tileClicked) && _grid._tileHovered != -1;
        _isDragging = ctx.started || ctx.performed;
    }

    public void OnZoom(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _scrollAmount = ctx.ReadValue<float>();
        _isZooming = ctx.started || ctx.performed;
    }

    void LateUpdate()
    {
        if (_isDragging && _tileClicked)
        {
            _height = 2f * _cam.orthographicSize;
            _width = _height * _cam.aspect;
            _difference = GetMousePosition - transform.position;
            Vector3 newPos = _origin - _difference;

            //float offset = 10.24f * 0.1f;
            float xMin = 0f;
            float yMin = 0f;
            float xMax = 15f;
            float yMax = 15f;
            newPos.x = Mathf.Clamp(newPos.x, xMin, xMax);
            newPos.y = Mathf.Clamp(newPos.y, yMin, yMax);

            transform.position = newPos;
        }
        else if (_isZooming && _grid._tileHovered != -1)
        {
            //Debug.Log(_scrollAmount + " " + _zoom);
            _zoom -= _scrollAmount * _zoomMultiplier;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);

            //cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
            _cam.orthographicSize = _zoom;
        }
    }

    Vector3 GetMousePosition => _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
