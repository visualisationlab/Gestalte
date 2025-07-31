using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public SpriteRenderer image;
    public Sprite pointCursor;
    public Sprite dragCursor;
    public Sprite buildCursor;
    public bool realCursorVisible;
    [Header("Mouse Info")] 
    public GameObject infoObject;
    public TextMeshPro infoText;

    private void Start()
    {
        Cursor.visible = realCursorVisible;
    }

    public void SetToDrag()
    {
        image.sprite = dragCursor;
    }

    public void SetToPoint()
    {
        image.sprite = pointCursor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        ;
    }
}