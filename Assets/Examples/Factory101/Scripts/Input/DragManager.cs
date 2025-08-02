using UnityEngine;

[DisallowMultipleComponent]
public class DragManager : MonoBehaviour
{
    [SerializeField] private PointerRaycaster raycaster;
    [SerializeField] private InputService inputService;

    private IDraggable _current;
    private Camera _camera;
    private bool _isDragging;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        raycaster.Clicked.AddListener(OnPointerDown);
        inputService.OnClickCanceled.AddListener(OnPointerUp);
    }

    private void OnDisable()
    {
        raycaster.Clicked.RemoveListener(OnPointerDown);
        inputService.OnClickCanceled.RemoveListener(OnPointerUp);
    }

    private void Update()
    {
        if (_isDragging && _current != null)
        {
            if (UIUtils.IsPointerOverUI())
            {
                // Optionally suspend dragging if you want UI to interrupt
                return;
            }

            Vector3 worldPos = GetPointerWorldPosition();
            _current.UpdateDrag(worldPos);
        }
    }

    private void OnPointerDown(GameObject clicked)
    {
        // Block if over UI
        if (UIUtils.IsPointerOverUI())
            return;

        if (clicked.TryGetComponent<IDraggable>(out var draggable))
        {
            _current = draggable;
            _isDragging = true;
            Vector3 worldPos = GetPointerWorldPosition();
             _current.StartDrag(worldPos);
        }
        else
        {
            // clicked something else; could cancel existing drag
            EndDrag();
        }
    }

    private void OnPointerUp()
    {
        EndDrag();
    }

    private void EndDrag()
    {
        if (_isDragging && _current != null)
        {
            _current.StopDrag();
        }

        _current = null;
        _isDragging = false;
    }

    private Vector3 GetPointerWorldPosition()
    {
        Vector2 screenPos;

#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            screenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        else
            screenPos = Input.mousePosition;
#else
        screenPos = Input.mousePosition;
#endif

        Vector3 world = _camera.ScreenToWorldPoint(screenPos);
        world.z = 0; // adjust as appropriate
        return world;
    }
}
