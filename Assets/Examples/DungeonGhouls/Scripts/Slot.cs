using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Sprite deselectSprite;
    public Sprite selectSprite;
    public Image slotImage;
    
    [ContextMenu("Select")]
    public void Select()
    {
        slotImage.sprite = selectSprite;
    }
    [ContextMenu("Deselect")]
    public void Deselect()
    {
        slotImage.sprite = deselectSprite;
    }
    
}
