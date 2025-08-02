using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public static class UIUtils
{
    /// <summary>
    /// Returns true if the current pointer (mouse or primary touch) is over a UI element that blocks raycasts.
    /// </summary>
    public static bool IsPointerOverUI()
    {
        if (EventSystem.current == null) 
            return false;

        Vector2 screenPos = GetPointerScreenPosition();

        var ped = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }

    private static Vector2 GetPointerScreenPosition()
    {
#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }
        // Fallback to old input if new system not present
#endif
        return Input.mousePosition;
    }
}