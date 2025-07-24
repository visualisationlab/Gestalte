using Director;
using GraphTool.Runtime;
using UnityEngine;

[ExecuteInEditMode]
public class CorrelatorGraphDebug : MonoBehaviour
{
    public Correlator correlator;
    public Color trackColor;
    
    void OnEnable()
    {
        GraphSubscription.Subscribe(
            this,
            () => correlator.Evaluate(),
            trackColor
        );
    }
    
    void OnDisable()
    {
        GraphSubscription.Unsubscribe(this);
    }

    void OnDestroy()
    {
        GraphSubscription.Unsubscribe(this);
    }

}
