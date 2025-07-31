using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Toggle))]
public class UIOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHoverExit?.Invoke();
    }
}