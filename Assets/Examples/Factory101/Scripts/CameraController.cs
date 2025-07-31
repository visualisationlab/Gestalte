using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Speeds")]
    public float moveSpeed = 10f;
    public float edgeScrollSpeed = 10f;
    public int edgeThickness = 10;

    [Header("World Bounds")]
    public Vector2 minBounds = new Vector2(-20f, -20f);
    public Vector2 maxBounds = new Vector2( 20f,  20f);

    [Header("Debug Freeze")]
    public bool freezeCamera = false;

    void Update()
    {
        if (freezeCamera) return;

        // 1) Keyboard input
        Vector2 kb = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)    kb.y += 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)  kb.y -= 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)  kb.x -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) kb.x += 1;

        // 2) Build target pos
        Vector3 pos = transform.position;
        pos += (Vector3)kb.normalized * moveSpeed * Time.deltaTime;

        // 3) Edge scrolling
        Vector2 m = Mouse.current.position.ReadValue();
        if (m.x >= Screen.width  - edgeThickness) pos.x += edgeScrollSpeed * Time.deltaTime;
        if (m.x <= edgeThickness)          pos.x -= edgeScrollSpeed * Time.deltaTime;
        if (m.y >= Screen.height - edgeThickness) pos.y += edgeScrollSpeed * Time.deltaTime;
        if (m.y <= edgeThickness)          pos.y -= edgeScrollSpeed * Time.deltaTime;

        // 4) Clamp to world limits
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

        // 5) Only move & log if different
        if (pos != transform.position)
        {
            transform.position = pos;
        }
    }

    public void SetFreezeCamera(bool state)
    {
        freezeCamera = state;
    }
}
