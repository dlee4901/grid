using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineCamera _cam;
    int _tileHovered;

    float _zoomStartMultiplier = 7f;
    float _minX;
    float _maxX;
    float _minY;
    float _maxY;
    float _minZoom;
    float _maxZoom;
    float _zoomMultiplier;

    bool _isDragging;
    bool _tileClicked;
    float _zoom;
    float _scrollAmount;
    Vector3 _origin;
    Vector3 _difference;

    void Start()
    {
        EventManager.Singleton.TileHoverEvent += TileHover;
        _tileHovered = 0;
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

    public void Init(GridManager grid)
    {
        (int x, int y, float tileScale) = grid.GetSize();
        _minX = 0;
        _minY = 0;
        _maxX = 10 * (x - 1) * tileScale;
        _maxY = 10 * (y - 1) * tileScale;
        _minZoom = Mathf.Max(x, y) * tileScale;
        _maxZoom = Mathf.Max(x, y) * tileScale * _zoomStartMultiplier * 2f;
        _zoomMultiplier = (_maxZoom / _minZoom) * tileScale / 2f;
        
        _cam.Lens.OrthographicSize = Mathf.Max(x, y) * tileScale * _zoomStartMultiplier;
        _zoom = _cam.Lens.OrthographicSize;
        transform.position = Util.Get2DWorldPos(new Vector3Int(x/2, y/2, 0), tileScale);
    }

    public void OnSelect(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = Util.GetMousePosition();
        _tileClicked = (ctx.started || _tileClicked) && _tileHovered != 0;
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
        if (_tileHovered != 0)
        {
            _zoom -= _scrollAmount * _zoomMultiplier;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
            _cam.Lens.OrthographicSize = Mathf.Lerp(_cam.Lens.OrthographicSize, _zoom, Time.deltaTime * 10f);
        }
    }
}
