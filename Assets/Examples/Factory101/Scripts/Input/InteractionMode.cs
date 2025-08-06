using UnityEngine;
using UnityEngine.Events;

public class InteractionModeController : MonoBehaviour
{
    public static InteractionModeController Instance { get; private set; }

    public InteractionMode CurrentMode { get; private set; } = InteractionMode.Default;
    public UnityEvent<InteractionMode> OnModeChanged;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void SetModePlacement(bool activate)
    {
        if (!activate)
        {
            SetMode(InteractionMode.Default);
            return;
        }
        SetMode(InteractionMode.Placement);
    }

    public void SetModeDefault()
    {
        SetMode(InteractionMode.Default);
    }
    
    public void SetModeBlocked()
    {
        SetMode(InteractionMode.Blocked);
    }

    public void SetMode(InteractionMode mode)
    {
        if (CurrentMode == mode) return;
        CurrentMode = mode;
        OnModeChanged?.Invoke(mode);
    }
    
}

public enum InteractionMode
{
    Blocked,
    Default,
    Placement
}