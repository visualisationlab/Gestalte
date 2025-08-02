using UnityEngine;

public class GlobalMrMcGibbleTracker : MonoBehaviour
{
    public static GlobalMrMcGibbleTracker Instance { get; private set; }

    public ConsumeMachine consumeMachine;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowArrow()
    {
        consumeMachine.ShowArrow();
    }

    public void HideArrow()
    {
        consumeMachine.HideArrow();
    }
}
