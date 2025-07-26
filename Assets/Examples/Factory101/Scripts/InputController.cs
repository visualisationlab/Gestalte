using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Mouse Interactions")]
    public InputActionReference useAction;
    public UnityEvent<GameObject> OnClickedGameObject;
    public UnityEvent<GameObject> OnHoverGameObject;
    public UnityEvent OnHoverOut;
    private Vector2 mouseWorldPos2D;
    private RaycastHit2D hit;

    private bool hovering;
    
    private void OnEnable()
    {
        useAction.action.started += OnUse;
        useAction.action.Enable();
    }

    private void OnDisable()
    {
        useAction.action.started -= OnUse;
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
        else if(hovering)
        {
            OnHoverOut.Invoke();
            hovering = false;
        }
    }

    private void OnUse(InputAction.CallbackContext ctx)
    {
        if (hit.collider != null)
        {
            OnClickedGameObject.Invoke(hit.collider.gameObject);
        }
    }
    
}