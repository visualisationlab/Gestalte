using UnityEngine;
using GraphTool.Runtime;

public class GraphToolTester : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float slider = 50.0f;

    void Start()
    {
        // A ping‑ponging value between 0 and 100
        GraphSubscription.Subscribe(
            "PingPong",
            () => Mathf.PingPong(Time.time * 20f, 100f),
            Color.cyan
        );

        // Update the slider value in the GraphSubscription
        GraphSubscription.Subscribe(
            "SliderValue",
            () => slider,
            Color.green
        );
    }

    void OnDestroy()
    {
        GraphSubscription.Unsubscribe("PingPong");
        GraphSubscription.Unsubscribe("SliderValue");
    }
}
