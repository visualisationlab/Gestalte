using Examples.Factory101.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;

#region Optional interfaces for decoupling
public interface IHoverable
{
    void OnHoverEnter();
    void OnHoverExit();
}

public interface IClickable
{
    void OnClicked();
}
#endregion

[DisallowMultipleComponent]
public class PointerRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    [Tooltip("Layers that will be considered for hover/click.")]
    public LayerMask interactableLayers = ~0; // default all

    [Tooltip("If true, uses Physics.Raycast (3D); otherwise uses Physics2D.Raycast.")]
    public bool use3D = false;

    [Tooltip("Maximum distance for 3D raycasts. Ignored in 2D.")]
    public float maxDistance = 100f;
    
    [Header("Events")]
    // Events: subscribers can react
    public UnityEvent<GameObject> HoverEnter;
    public UnityEvent<GameObject> HoverExit;
    public UnityEvent<GameObject> Clicked; // clicked on a target
    public UnityEvent ClickedOutside;      // clicked but nothing hit

    // Internal state
    private GameObject _currentHover;
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
        if (_camera == null)
            Debug.LogError("[PointerRaycaster] No main camera found. Cache a camera or assign one explicitly.");
    }

    void Update()
    {
        UpdateHover();
    }

    private void UpdateHover()
    {
        if (UIUtils.IsPointerOverUI())
        {
            return;
        }
        
        GameObject newHover = RaycastUnderPointer();

        if (newHover != _currentHover)
        {
            // Hover exit on previous
            if (_currentHover != null)
            {
                HoverExit?.Invoke(_currentHover);
                if (_currentHover.TryGetComponent<IHoverable>(out var prevHoverable))
                    prevHoverable.OnHoverExit();
            }

            // Hover enter on new
            if (newHover != null)
            {
                HoverEnter?.Invoke(newHover);
                if (newHover.TryGetComponent<IHoverable>(out var hoverable))
                    hoverable.OnHoverEnter();
            }

            _currentHover = newHover;
        }
    }

    /// <summary>
    /// Should be called by your input layer when a click/use action happens.
    /// </summary>
    public void ProcessClick()
    {
        if (UIUtils.IsPointerOverUI())
        {
            return;
        }
        
        GameObject hit = RaycastUnderPointer();
        if (hit != null)
        {
            Clicked?.Invoke(hit);
            if (hit.TryGetComponent<IClickable>(out var clickable))
                clickable.OnClicked();
        }
        else
        {
            ClickedOutside?.Invoke();
        }
    }

    /// <summary>
    /// Performs raycast based on current pointer/mouse position and returns the hit GameObject (or null).
    /// </summary>
    private GameObject RaycastUnderPointer()
    {
        if (_camera == null)
            return null;

        Vector2 screenPos;

#if ENABLE_INPUT_SYSTEM && (UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL)
        // Using new Input System if available
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            screenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }
        else
        {
            screenPos = Input.mousePosition;
        }
#else
        Vector3 m = Input.mousePosition;
        screenPos = new Vector2(m.x, m.y);
#endif

        if (use3D)
        {
            Ray ray = _camera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out var hitInfo, maxDistance, interactableLayers))
            {
                return hitInfo.collider.gameObject;
            }
        }
        else
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);
            Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);
            var hit2D = Physics2D.Raycast(worldPos2D, Vector2.zero, Mathf.Infinity, interactableLayers);
            if (hit2D.collider != null)
                return hit2D.collider.gameObject;
        }

        return null;
    }
    public GameObject CurrentHover => _currentHover;

    public bool HoverOverBlockingMachine()
    {
        if (CurrentHover == null) return false;
        return CurrentHover.TryGetComponent<IBlockPlacement>(out _);
    }
}
