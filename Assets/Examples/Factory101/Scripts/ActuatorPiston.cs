using UnityEngine;

public class ActuatorPiston : MonoBehaviour
{
    public Rigidbody2D rb;
    [Tooltip("Local-space direction the piston pushes (e.g., right is forward).")]
    public Vector2 localDirection = Vector2.right;
    public float distance = .1f; // extension distance
    public float speed = 1f; // units per second

    private bool extending = false;

    public void Extend()   => extending = true;
    public void Retract()  => extending = false;

    private Vector2 staticBasePos;
    
    void Start()
    {
        Vector2 initialBase = rb.position;
        staticBasePos = initialBase;
    }
    
    
    void FixedUpdate()
    {
        if (rb == null) return;

        // Compute the world-space base point (anchor) and direction
        Vector2 worldDir = ((Vector2)transform.TransformDirection(localDirection)).normalized;

        // Determine desired target (extended or retracted)
        Vector2 targetPos = extending 
            ? staticBasePos + worldDir * distance 
            : staticBasePos;

        // Move toward it smoothly
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
}