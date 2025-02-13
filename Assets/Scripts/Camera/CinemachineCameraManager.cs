using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;

    private InputHandler _inputHandler;
    private int _tileHovered;

    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
    private float _minZoom;
    private float _maxZoom;
    private float _zoomSpeed;
    private float _zoom;
    private Vector3 _origin;
    private Vector3 _difference;

    private void Start()
    {
        _inputHandler = new InputHandler();
        _tileHovered = 0;

        EventManager.Singleton.TileHover += OnEventTileHover;
        _inputHandler.MoveCameraPerformed += OnInputMoveCameraPerformed;
        _inputHandler.ZoomCameraPerformed += OnInputZoomCameraPerformed;
    }

    private void LateUpdate()
    {
        //HandleCameraMovement();
        //HandleCameraZoom();
    }

    private void OnEventTileHover(int id)
    {
        _tileHovered = id;
    }

    private void OnInputMoveCameraPerformed()
    {
        Debug.Log("OnInputMoveCameraPerformed");
        if (_tileHovered != 0) HandleCameraMovement();
    }

    private void OnInputZoomCameraPerformed(float zoom)
    {
        Debug.Log("OnInputZoomCameraPerformed");
        if (_tileHovered != 0) HandleCameraZoom(zoom);
    }

    public void Init(SquareGrid grid)
    {
        (int x, int y, float tileScale) = grid.GetSize();
        Debug.Log(grid.GetSize());
        _minX = 0;
        _minY = 0;
        _maxX = 10 * (x - 1) * tileScale;
        _maxY = 10 * (y - 1) * tileScale;
        _minZoom = Mathf.Max(x, y) * tileScale;
        _maxZoom = Mathf.Max(x, y) * tileScale * 14f;
        _zoomSpeed = 1f;
        
        _camera.Lens.OrthographicSize = Mathf.Max(x, y) * tileScale * 14f;
        _zoom = _camera.Lens.OrthographicSize;
        transform.position = Util.Get2DWorldPos(new Vector3Int(x/2, y/2, 0), tileScale);

        Debug.Log(_minZoom);
        Debug.Log(_maxZoom);
        Debug.Log(_camera.Lens.OrthographicSize);
        Debug.Log(_zoom);
    }

    private void CenterCameraToGrid()
    {

    }

    // public void OnSelect(InputAction.CallbackContext ctx)
    // {
    //     if (ctx.started) _origin = Util.GetMousePosition();
    //     _tileClicked = (ctx.started || _tileClicked) && _tileHovered != 0;
    //     _isDragging = ctx.started || ctx.performed;
    // }

    // public void OnZoom(InputAction.CallbackContext ctx)
    // {
    //     if (ctx.started) _origin = Util.GetMousePosition();
    //     _scrollAmount = ctx.ReadValue<float>();
    // }

    private void HandleCameraMovement()
    {
        Debug.Log("HandleCameraMovement");
        Vector3 target = Util.GetMousePosition();
        target.x = Mathf.Clamp(target.x, _minX, _maxX);
        target.y = Mathf.Clamp(target.y, _minY, _maxY);
        Debug.Log(transform.position);
        Debug.Log(target);
        StartCoroutine(Util.LerpTransform(transform, transform.position, target, 0.5f));
        // _origin = Util.GetMousePosition();
        // _difference = Util.GetMousePosition() - transform.position;
        // Vector3 newPos = _origin - _difference;
        // newPos.x = Mathf.Clamp(newPos.x, _minX, _maxX);
        // newPos.y = Mathf.Clamp(newPos.y, _minY, _maxY);
        // transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 10f);

        // Vector3 mouseDelta = GetMousePosition - _origin;
        // Vector3 newPos = new Vector3(0, 0, 0);
        // newPos.x = Mathf.Clamp(transform.position.x - mouseDelta.x, _minX, _maxX);
        // newPos.y = Mathf.Clamp(transform.position.y - mouseDelta.y, _minY, _maxY);
        // transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
    }

    private void HandleCameraZoom(float zoom)
    {
        Debug.Log("HandleCameraZoom");
        Debug.Log(_minZoom);
        Debug.Log(_maxZoom);
        if (zoom < 0)      zoom = -1;
        else if (zoom > 0) zoom = 1;
        _zoom -= zoom * _zoomSpeed;
        Debug.Log(zoom);
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        _camera.Lens.OrthographicSize = _zoom; //Mathf.Lerp(_camera.Lens.OrthographicSize, _zoom, Time.deltaTime * 10f);
    }
}
