using UnityEngine;

public class LightMachine : MonoBehaviour
{
    [SerializeField] private SpriteRenderer lightImage;

    private void Start()
    {
        SetState(false);
    }

    public void SetState(bool state)
    {
        lightImage.color = state ? Color.yellow: Color.grey;
    }
}
