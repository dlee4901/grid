using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    bool _isDragging;
    bool _isZooming;
    bool _tileClicked;

    public float _minX = 0f;
    public float _maxX = 70f;
    public float _minY = 0f;
    public float _maxY = 70f;
    public float _zoomMultiplier = 5f;
    public float _minZoom = 10f;
    public float _maxZoom = 100f;

    float _zoom;
    float _scrollAmount;
    Vector3 _origin;
    Vector3 _difference;

    int _tileHovered;

    void Start()
    {
        EventManager.Singleton.TileHoverEvent += TileHover;
        _tileHovered = -1;
        _zoom = cinemachineCamera.Lens.OrthographicSize;
    }

    void LateUpdate()
    {
        HandleCameraMovement();
        HandleCameraZoom();
    }

    void TileHover(int id)
    {
        _tileHovered = id;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _tileClicked = (ctx.started || _tileClicked) && _tileHovered != -1;
        _isDragging = ctx.started || ctx.performed;
    }

    public void OnZoom(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _scrollAmount = ctx.ReadValue<float>();
        _isZooming = ctx.started || ctx.performed;
    }

    void HandleCameraMovement()
    {
        if (_isDragging && _tileClicked)
        {
            _difference = GetMousePosition - transform.position;
            Vector3 newPos = _origin - _difference;
            newPos.x = Mathf.Clamp(newPos.x, _minX, _maxX);
            newPos.y = Mathf.Clamp(newPos.y, _minY, _maxY);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 10f);

            // Vector3 mouseDelta = GetMousePosition - _origin;
            // Vector3 newPos = new Vector3(0, 0, 0);
            // newPos.x = Mathf.Clamp(transform.position.x - mouseDelta.x, _minX, _maxX);
            // newPos.y = Mathf.Clamp(transform.position.y - mouseDelta.y, _minY, _maxY);
            // transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
        }
    }

    void HandleCameraZoom()
    {
        if (_tileHovered != -1)
        {
            _zoom -= _scrollAmount * _zoomMultiplier;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, _zoom, Time.deltaTime * 10f);
        }
    }

    Vector3 GetMousePosition => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
