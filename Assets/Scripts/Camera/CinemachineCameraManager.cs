using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineCameraManager : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;

    float _minX = 0f;
    float _maxX = 70f;
    float _minY = 0f;
    float _maxY = 70f;
    float _zoomMultiplier = 5f;
    float _minZoom = 10f;
    float _maxZoom = 100f;

    bool _isDragging;
    bool _tileClicked;
    float _zoom;
    float _scrollAmount;
    Vector3 _origin;
    Vector3 _difference;

    int _tileHovered;

    void Start()
    {
        EventManager.Singleton.TileHoverEvent += TileHover;
        _tileHovered = -1;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Min(cinemachineCamera.Lens.OrthographicSize, _maxZoom);
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

    public void Init(int x, int y)
    {
        _minX = 0;
        _minY = 0;
        _maxX = 10 * (x - 1);
        _maxY = 10 * (y - 1);
        _minZoom = 10f;
        _maxZoom = Mathf.Max(x, y) * 10f;
        _zoomMultiplier = (_maxZoom / _minZoom) / 2f;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = Util.GetMousePosition();
        _tileClicked = (ctx.started || _tileClicked) && _tileHovered != -1;
        _isDragging = ctx.started || ctx.performed;
    }

    public void OnZoom(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = Util.GetMousePosition();
        _scrollAmount = ctx.ReadValue<float>();
    }

    void HandleCameraMovement()
    {
        if (_isDragging && _tileClicked)
        {
            _difference = Util.GetMousePosition() - transform.position;
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
}
