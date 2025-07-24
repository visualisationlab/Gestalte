using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public InputActionReference moveAction;
    private Rigidbody2D rb;
    
    private Vector2 velocity;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float deadzone = 0.2f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction.action.performed += c => Move(c);
        moveAction.action.canceled += c => StopMoving();
        moveAction.action.Enable();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = velocity * (speed * Time.fixedDeltaTime);
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input.magnitude < deadzone)
        {
            StopMoving();
        }
        else
        {
            velocity = input;
        }
    }

    private void StopMoving()
    {
        velocity = Vector2.zero;
    }
}
