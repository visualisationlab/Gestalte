using UnityEngine;

public class InputStateMachine : MonoBehaviour
{
    public enum InputState
    {
        Interact,
        Build
    }

    public InputState state;

    public InputController interactInput;
    public InputController buildInput;
    
    

    private void Start()
    {
        SetState(state);
    }

    public void SetInteractState()
    {
        SetState(InputState.Interact);
    }
    
    public void SetBuildState()
    {
        SetState(InputState.Build);
    }
    
    private void SetState(InputState newState)
    {
        buildInput.enabled = newState == InputState.Build;
        interactInput.enabled = newState == InputState.Interact;
        state = newState;
        Debug.Log($"Set Input State: {state}");
    }
}
