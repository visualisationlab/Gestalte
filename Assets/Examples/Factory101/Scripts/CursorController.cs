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

    public BuildToolGhost placeableGhost;
    
    public static CursorController Instance { get; private set; }
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // or Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject); // optional
    }
    
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
    
    public void SetToBuild()
    {
        image.sprite = buildCursor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }

    public void ShowMouseInfo(string message, Color textColor)
    {
        infoObject.SetActive(true);
        infoText.color = textColor;
        infoText.text = message;
    }
    
    public void HideMouseInfo()
    {
        infoText.text = "";
        infoText.color = Color.white;
        infoObject.SetActive(false);
    }

    public void ShowBuildGhost(GameObject ghost, bool canRotate)
    {
        placeableGhost.gameObject.SetActive(true);
        placeableGhost.SetGhost(ghost, canRotate);
        SetToBuild();
    }
    
    public void HideBuildGhost()
    {
        placeableGhost.RemoveGhost();
        placeableGhost.gameObject.SetActive(false);
        SetToPoint();
    }

    public void SetGhostRotation(float rotation)
    {
        placeableGhost.RotateGhost(rotation);
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public Vector3 GetPositionInGrid()
    {
        return new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);
    }

}