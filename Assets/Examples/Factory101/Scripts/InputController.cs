using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Mouse Interactions")]
    public InputActionReference useAction;
    public UnityEvent<GameObject> OnClickedGameObject;
    public UnityEvent<GameObject> OnHoverGameObject;
    public UnityEvent OnHoverOut;
    public UnityEvent OnClickedOutside;
    private Vector2 mouseWorldPos2D;
    private RaycastHit2D hit;

    private bool hovering;
    [SerializeField] private Draggable currentDraggable;

    private void OnEnable()
    {
        useAction.action.started += OnUse;
        useAction.action.canceled += OnUseCanceled;
        useAction.action.Enable();
    }

    private void OnDisable()
    {
        useAction.action.started -= OnUse;
        useAction.action.canceled -= OnUseCanceled;
        useAction.action.Disable();
    }

    public void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        mouseWorldPos2D = new Vector2(worldPos.x, worldPos.y);
        hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero);
        OnHover();
    }

    private void OnHover()
    {
        if (hit.collider != null)
        {
            hovering = true;
            OnHoverGameObject.Invoke(hit.collider.gameObject);
        }
        else if (hovering)
        {
            OnHoverOut?.Invoke();
            hovering = false;
        }
    }

    private void OnUse(InputAction.CallbackContext ctx)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            // Ignore clicks over UI
            return;
        }

        if (hit.collider != null)
        {
            // Found machine
            if (hit.collider.gameObject.GetComponent<ExposeMachine>())
            {
                OnClickedGameObject.Invoke(hit.collider.gameObject);
            }
            else
            {
                Draggable draggable = hit.collider.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    worldPos.z = 0;
                    draggable.StartDragging(worldPos);
                    currentDraggable = draggable;
                }
            }
        }
        else
        {
            OnClickedOutside.Invoke();
        }
    }
    
    private void OnUseCanceled(InputAction.CallbackContext ctx)
    {
        if (currentDraggable != null)
        {
            currentDraggable.StopDragging();
            currentDraggable = null;
        }
    }
}
