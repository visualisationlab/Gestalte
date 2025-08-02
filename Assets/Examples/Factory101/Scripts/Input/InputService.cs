using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour
{
    public UnityEvent OnClick;
    public UnityEvent OnClickCanceled;
    public UnityEvent OnRotate;
    public UnityEvent OnCancel;

    [SerializeField] private InputActionReference _use;
    [SerializeField] private InputActionReference _cancel;
    [SerializeField] private InputActionReference _rotate;

    private void OnEnable()
    {
        _use.action.started += ctx => OnClick?.Invoke();
        _use.action.canceled += ctx => OnClickCanceled?.Invoke();
        _use.action.Enable();
        _cancel.action.performed += ctx => OnCancel?.Invoke();
        _cancel.action.Enable();
        _rotate.action.started += ctx => OnRotate?.Invoke();
        _rotate.action.Enable();
    }

}