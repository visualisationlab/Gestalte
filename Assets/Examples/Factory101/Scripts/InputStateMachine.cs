using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class InputStateMachine : MonoBehaviour
{
    public enum InputState
    {
        Interact,
        Build
    }

    public InputState state;

    public InputController interactInput;
    public InputController toolInput;
    public CursorController cursor;

    private void Start()
    {
        StartCoroutine(DelayedEnable());
    }


    //UGLY Fix For Input Interact not always working from the very beginning
    public IEnumerator DelayedEnable()
    {
        SetState(InputState.Build);
        yield return new WaitForSeconds(.2f);
        SetState(InputState.Interact);
    }

    public void SetInteractState()
    {
        SetState(InputState.Interact);
    }

    public void SetBuildState()
    {
        SetState(InputState.Build);
        cursor.SetToBuild();
    }

    private void SetState(InputState newState)
    {
        toolInput.enabled = newState == InputState.Build;
        interactInput.enabled = newState == InputState.Interact;
        state = newState;
        Debug.Log($"Set Input State: {state}");
    }
    
    public bool isBuilding()
    {
        return state == InputState.Build;
    }
}
