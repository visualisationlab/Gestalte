using UnityEngine;
using GraphTool.Runtime;

public class GraphToolTester : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float slider = .5f;

    void Start()
    {
        // A ping‑ponging value between 0 and 100
        GraphSubscription.Subscribe(
            this,
            () => Mathf.PingPong(Time.time * 20f, 100f),
            Color.cyan
        );

        // Update the slider value in the GraphSubscription
        GraphSubscription.Subscribe(
            this,
            () => slider,
            Color.green
        );
    }

    void OnDestroy()
    {
        GraphSubscription.Unsubscribe(this);
        GraphSubscription.Unsubscribe(this);
    }
}
